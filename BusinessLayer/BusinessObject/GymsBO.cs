using AutoMapper;
using DataLayer.Entities;
using DataLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace BusinessLayer.BusinessObject
{
    public class GymsBO: BaseBusinessObject
    {
        public int Id { get; set; }
        public string GymName { get; set; }

        //------------------------------------------------------------------------------------
        readonly IUnityContainer unityContainer;
        public GymsBO(IMapper mapper, UnitOfWork unitOfWork, IUnityContainer container)
            : base(mapper, unitOfWork)
        {
            unityContainer = container;
        }
        public IEnumerable<GymsBO> LoadAll()  //из DataObj в BusinessObj
        {
            var managers = unitOfWork.Gyms.GetAll();
            var res = managers.AsEnumerable().Select(a => mapper.Map<GymsBO>(a)).ToList();
            //res.ForEach(r => System.Diagnostics.Debug.WriteLine(r.Login));
            return res;
        }

        public GymsBO Load(int id)
        {
            var managers = unitOfWork.Gyms.GetById(id);
            return mapper.Map(managers, this);
        }
        public void Save(GymsBO coacheBO)
        {
            var coache = mapper.Map<Gyms>(coacheBO);
            if (coacheBO.Id == 0) {
                Add(coache);
            }
            else {
                Update(coache);
            }
            unitOfWork.Gyms.Save();
        }
        private void Add(Gyms coache)
        {
            unitOfWork.Gyms.Create(coache);
        }
        private void Update(Gyms coache)
        {
            unitOfWork.Gyms.Update(coache);
        }
        public void DeleteSave(GymsBO coacheBO)
        {
            var coache = mapper.Map<Gyms>(coacheBO);
            unitOfWork.Gyms.Delete(coache.Id);
            unitOfWork.Gyms.Save();
        }
    }
}
