using Microsoft.AspNetCore.Mvc;
using SlowGas.Models.BusinessLogicsContracts;
using SlowGas.Models.DataModels;

namespace SlowGas.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalaryController : ControllerBase
    {
        private readonly ISalaryBusinessLogic _salaryBusinessLogic;
        private readonly ILogger<SalaryController> _logger;

        public SalaryController(ISalaryBusinessLogic salaryBusinessLogic, ILogger<SalaryController> logger)
        {
            _salaryBusinessLogic = salaryBusinessLogic;
            _logger = logger;
        }

        [HttpGet("by-period")]
        public IActionResult GetSalariesByPeriod(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var result = _salaryBusinessLogic.GetAllSalariesByPeriod(fromDate, toDate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting salaries by period");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("by-worker")]
        public IActionResult GetSalariesByWorker(DateTime fromDate, DateTime toDate, string workerId)
        {
            try
            {
                var result = _salaryBusinessLogic.GetAllSalariesByPeriodByWorker(fromDate, toDate, workerId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting salaries by worker");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("calculate/{year}/{month}")]
        public IActionResult CalculateSalary(int year, int month)
        {
            try
            {
                var date = new DateTime(year, month, 1);
                _salaryBusinessLogic.CalculateSalaryByMonth(date);
                return Ok($"Salary calculation for {year}-{month} started");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating salary");
                return BadRequest(ex.Message);
            }
        }
    }
}