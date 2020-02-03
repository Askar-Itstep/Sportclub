using AutoMapper;
using DataLayer.Entities;
using DataLayer.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace BusinessLayer.BusinessObject
{
    public class AdministrationBO: BaseBusinessObject
    {
        public int Id { get; set; }

        public int UserBOId { get; set; }
        public UserBO UserBO { get; set; }

        [Required]
        public StatusManager Status { get; set; }

        public enum StatusManager
        {
            ADMIN, MANAGER, TOP_MANAGER
        }
        //------------------------------------------------------------------------------------------

        readonly IUnityContainer unityContainer;
        public AdministrationBO(IMapper mapper, UnitOfWork unitOfWork, IUnityContainer container)
            : base(mapper, unitOfWork)
        {
            unityContainer = container;
        }
        public IEnumerable<AdministrationBO> LoadAll()  //из DataObj в BusinessObj
        {
            var managers = unitOfWork.Administration.GetAll();
            var res = managers.AsEnumerable().Select(a => mapper.Map<AdministrationBO>(a)).ToList();
            return res;
        }
        public IEnumerable<AdministrationBO> LoadAllWithInclude(params string[] properties )  //из DataObj в BusinessObj
        {
            var managers = unitOfWork.Administration.Include(properties);
            var res = managers.AsEnumerable().Select(a => mapper.Map<AdministrationBO>(a)).ToList();
            return res;
        }
        public void  Load(int id)
        {
            var managers = unitOfWork.Administration.GetById(id);
            mapper.Map(managers, this);
        }
        public void Save(AdministrationBO adminBO)
        {
            var admin = mapper.Map<Administration>(adminBO);
            if (adminBO.Id == 0) {
                Add(admin);
            }
            else {
                Update(admin);
            }
            unitOfWork.Administration.Save();
        }
        private void Add(Administration admin)
        {
            unitOfWork.Administration.Create(admin);
        }
        private void Update(Administration admin)
        {
            unitOfWork.Administration.Update(admin);
        }
        public void DeleteSave(AdministrationBO adminBO)
        {
            var admin = mapper.Map<Administration>(adminBO);
            unitOfWork.Administration.Delete(admin.Id);
            unitOfWork.Administration.Save();
        }
    }
}
