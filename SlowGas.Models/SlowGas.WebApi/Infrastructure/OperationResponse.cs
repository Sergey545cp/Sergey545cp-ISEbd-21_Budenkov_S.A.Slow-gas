using Microsoft.AspNetCore.Mvc;
using SlowGas.Models.ViewModels;
using System.Net;

namespace SlowGas.WebApi.Infrastructure
{
    public class OperationResponse
    {
        protected HttpStatusCode StatusCode { get; set; }
        protected object? Result { get; set; }

        public IActionResult GetResponse(HttpRequest request, HttpResponse response)
        {
            response.StatusCode = (int)StatusCode;
            if (Result is null)
            {
                return new StatusCodeResult((int)StatusCode);
            }
            return new ObjectResult(Result);
        }

        protected static TResult OK<TResult, TData>(TData data) where TResult : OperationResponse, new()
            => new() { StatusCode = HttpStatusCode.OK, Result = data };

        protected static TResult NoContent<TResult>() where TResult : OperationResponse, new()
            => new() { StatusCode = HttpStatusCode.NoContent };

        protected static TResult BadRequest<TResult>(string? errorMessage = null) where TResult : OperationResponse, new()
            => new() { StatusCode = HttpStatusCode.BadRequest, Result = errorMessage };

        protected static TResult NotFound<TResult>(string? errorMessage = null) where TResult : OperationResponse, new()
            => new() { StatusCode = HttpStatusCode.NotFound, Result = errorMessage };

        protected static TResult InternalServerError<TResult>(string? errorMessage = null) where TResult : OperationResponse, new()
            => new() { StatusCode = HttpStatusCode.InternalServerError, Result = errorMessage };
    }

    public class CustomerOperationResponse : OperationResponse
    {
        public static CustomerOperationResponse OK(List<CustomerViewModel> data)
            => OK<CustomerOperationResponse, List<CustomerViewModel>>(data);
        public static CustomerOperationResponse OK(CustomerViewModel data)
            => OK<CustomerOperationResponse, CustomerViewModel>(data);
        public static CustomerOperationResponse NoContent()
            => NoContent<CustomerOperationResponse>();
        public static CustomerOperationResponse NotFound(string message)
            => NotFound<CustomerOperationResponse>(message);
        public static CustomerOperationResponse BadRequest(string message)
            => BadRequest<CustomerOperationResponse>(message);
        public static CustomerOperationResponse InternalServerError(string message)
            => InternalServerError<CustomerOperationResponse>(message);
    }

    public class MotorOperationResponse : OperationResponse
    {
        public static MotorOperationResponse OK(List<MotorViewModel> data)
            => OK<MotorOperationResponse, List<MotorViewModel>>(data);
        public static MotorOperationResponse OK(MotorViewModel data)
            => OK<MotorOperationResponse, MotorViewModel>(data);
        public static MotorOperationResponse NoContent()
            => NoContent<MotorOperationResponse>();
        public static MotorOperationResponse NotFound(string message)
            => NotFound<MotorOperationResponse>(message);
        public static MotorOperationResponse BadRequest(string message)
            => BadRequest<MotorOperationResponse>(message);
        public static MotorOperationResponse InternalServerError(string message)
            => InternalServerError<MotorOperationResponse>(message);
    }

    public class RequestOperationResponse : OperationResponse
    {
        public static RequestOperationResponse OK(List<RequestViewModel> data)
            => OK<RequestOperationResponse, List<RequestViewModel>>(data);
        public static RequestOperationResponse OK(RequestViewModel data)
            => OK<RequestOperationResponse, RequestViewModel>(data);
        public static RequestOperationResponse NoContent()
            => NoContent<RequestOperationResponse>();
        public static RequestOperationResponse NotFound(string message)
            => NotFound<RequestOperationResponse>(message);
        public static RequestOperationResponse BadRequest(string message)
            => BadRequest<RequestOperationResponse>(message);
        public static RequestOperationResponse InternalServerError(string message)
            => InternalServerError<RequestOperationResponse>(message);
    }

    public class ShipmentOperationResponse : OperationResponse
    {
        public static ShipmentOperationResponse OK(List<ShipmentViewModel> data)
            => OK<ShipmentOperationResponse, List<ShipmentViewModel>>(data);
        public static ShipmentOperationResponse OK(ShipmentViewModel data)
            => OK<ShipmentOperationResponse, ShipmentViewModel>(data);
        public static ShipmentOperationResponse NoContent()
            => NoContent<ShipmentOperationResponse>();
        public static ShipmentOperationResponse NotFound(string message)
            => NotFound<ShipmentOperationResponse>(message);
        public static ShipmentOperationResponse BadRequest(string message)
            => BadRequest<ShipmentOperationResponse>(message);
        public static ShipmentOperationResponse InternalServerError(string message)
            => InternalServerError<ShipmentOperationResponse>(message);
    }

    public class InvoiceOperationResponse : OperationResponse
    {
        public static InvoiceOperationResponse OK(List<InvoiceViewModel> data)
            => OK<InvoiceOperationResponse, List<InvoiceViewModel>>(data);
        public static InvoiceOperationResponse OK(InvoiceViewModel data)
            => OK<InvoiceOperationResponse, InvoiceViewModel>(data);
        public static InvoiceOperationResponse NoContent()
            => NoContent<InvoiceOperationResponse>();
        public static InvoiceOperationResponse NotFound(string message)
            => NotFound<InvoiceOperationResponse>(message);
        public static InvoiceOperationResponse BadRequest(string message)
            => BadRequest<InvoiceOperationResponse>(message);
        public static InvoiceOperationResponse InternalServerError(string message)
            => InternalServerError<InvoiceOperationResponse>(message);
    }
}