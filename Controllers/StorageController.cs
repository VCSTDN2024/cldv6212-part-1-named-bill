using azuresolution1.Models;
using azuresolution1.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace azuresolution1.Controllers
{
    public class StorageController : Controller
    {
        private readonly TableStorageService _customerTableService;
        private readonly TableStorageService _productTableService;
        private readonly BlobStorageService _blobService;
        private readonly QueueStorageService _queueService;
        private readonly FileStorageService _fileService;

        public StorageController(
            TableStorageService customerTableService,
            TableStorageService productTableService,
            BlobStorageService blobService,
            QueueStorageService queueService,
            FileStorageService fileService)
        {
            _customerTableService = customerTableService;
            _productTableService = productTableService;
            _blobService = blobService;
            _queueService = queueService;
            _fileService = fileService;
        }

        public IActionResult Index()
        {
            ViewBag.Message = TempData["SuccessMessage"];
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer(CustomerProfile model)
        {
            if (ModelState.IsValid)
            {
                model.PartitionKey = "Customer";
                model.RowKey = Guid.NewGuid().ToString();
                await _customerTableService.AddEntityAsync(model);
                TempData["SuccessMessage"] = "Customer profile added successfully!";
                return RedirectToAction("Index");
            }
            return View("Index", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductInfo model, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                model.PartitionKey = "Product";
                model.RowKey = Guid.NewGuid().ToString();
                await _productTableService.AddEntityAsync(model);

                if (image != null && image.Length > 0)
                {
                    var blobName = $"{model.RowKey}_{image.FileName}";
                    using (var stream = image.OpenReadStream())
                    {
                        await _blobService.UploadBlobAsync(blobName, stream);
                    }
                }

                TempData["SuccessMessage"] = "Product added successfully!";
                return RedirectToAction("Index");
            }
            return View("Index", model);
        }

        [HttpPost]
        public async Task<IActionResult> ProcessOrder(string customerId, string productId)
        {
            var message = $"New order for Customer ID: {customerId}, Product ID: {productId}";
            await _queueService.SendMessageAsync(message);
            TempData["SuccessMessage"] = "Order processing message sent to queue.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UploadContract(IFormFile contract)
        {
            if (contract != null && contract.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}_{contract.FileName}";
                using (var stream = contract.OpenReadStream())
                {
                    await _fileService.UploadFileAsync(fileName, stream);
                }
                TempData["SuccessMessage"] = "Contract uploaded to Azure Files successfully!";
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ListData()
        {
            var customers = new List<CustomerProfile>();
            await foreach (var customer in _customerTableService.GetEntitiesAsync<CustomerProfile>())
            {
                customers.Add(customer);
            }

            var products = new List<ProductInfo>();
            await foreach (var product in _productTableService.GetEntitiesAsync<ProductInfo>())
            {
                products.Add(product);
            }

            ViewBag.Customers = customers;
            ViewBag.Products = products;

            return View();
        }
    }
}