using AutoMapper;
using SlowGas.Models.BindingModels;
using SlowGas.Models.DataModels;
using SlowGas.Models.ViewModels;
using SlowGas.Database.Models;  
namespace SlowGas.WebApi.Infrastructure
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Существующие маппинги для BindingModel -> DataModel
            CreateMap<CustomerBindingModel, Customer>();
            CreateMap<Customer, CustomerViewModel>();

            CreateMap<MotorBindingModel, Motor>();
            CreateMap<Motor, MotorViewModel>();

            CreateMap<RequestBindingModel, Request>();
            CreateMap<Request, RequestViewModel>();

            CreateMap<ShipmentBindingModel, Shipment>();
            CreateMap<Shipment, ShipmentViewModel>();

            CreateMap<InvoiceBindingModel, Invoice>();
            CreateMap<Invoice, InvoiceViewModel>();

            CreateMap<Customer, CustomerEntity>().ReverseMap();
            CreateMap<Motor, MotorEntity>().ReverseMap();
            CreateMap<Request, RequestEntity>().ReverseMap();  // если есть RequestEntity
            CreateMap<Shipment, ShipmentEntity>().ReverseMap();  // если есть ShipmentEntity
            CreateMap<Invoice, InvoiceEntity>().ReverseMap();  // если есть InvoiceEntity
        }
    }
}