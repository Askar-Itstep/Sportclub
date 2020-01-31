using AutoMapper;
using DataLayer.Repository;

namespace BusinessLayer.BusinessObject
{
    public class BaseBusinessObject
    {
        protected IMapper mapper;
        //UnitOfWorkFactory unitOfWorkFactory;

        protected UnitOfWork unitOfWork;

        public BaseBusinessObject(IMapper mapper, UnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }
    }
}