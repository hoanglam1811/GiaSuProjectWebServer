using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PRN231.Constant;
using PRN231.Models;
using PRN231.Models.DTOs;
using PRN231.Models.DTOs.Request;
using PRN231.Models.DTOs.Response;
using PRN231.Repository.Interfaces;
using PRN231.Services;
using PRN231.Services.Implementations;
using PRN231.Services.Interfaces;
using System.Net;

namespace PRN231.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IGenericService<Transaction, TransactionDTO> _transactionService;

        public TransactionController(IGenericService<Transaction, TransactionDTO> transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("GetAll")]
        //[Authorize]
        public async Task<IActionResult> GetAll()
        {
            var transactionList = await _transactionService.GetAll();
            return Ok(transactionList);
        }

        [HttpGet("Get")]
        //[Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var transactionList = await _transactionService.Get(id);
            return Ok(transactionList);
        }

        [HttpPost("GetAllByTypeTransaction")]
        public async Task<ActionResult<ApiResponse>> GetAllByTypeTransaction( string[] type)
        {
            try
            {
                var transactions = await _transactionService.GetAll();
                transactions = transactions.Where(x => type.Contains( x.Type));

                return Ok(new ApiResponse((int)HttpStatusCode.OK, MessageConstant.SUCCESSFUL, transactions));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest, MessageConstant.FAILED, null));
            }
        }

        [HttpGet("GetAllTransactionByUserId")]
        public async Task<ActionResult<ApiResponse>> GetAllTransactionByUserId(int userId)
        {
            try
            {
                var transactions = await _transactionService.GetAll();
                transactions = transactions.Where(x => x.UserId == userId || x.ReceiverId == userId);

                return Ok(new ApiResponse((int)HttpStatusCode.OK, MessageConstant.SUCCESSFUL, transactions));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest, MessageConstant.FAILED, null));
            }
        }

    }
}
