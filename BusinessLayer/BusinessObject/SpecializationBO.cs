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
    public class SpecializationBO: BaseBusinessObject
    {
        public int Id { get; set; }

        public string Title { get; set; }
        //------------------------------------------------------------------------------------------

        readonly IUnityContainer unityContainer;
        public SpecializationBO(IMapper mapper, UnitOfWork unitOfWork, IUnityContainer container)
            : base(mapper, unitOfWork)
        {
            unityContainer = container;
        }
        public IEnumerable<SpecializationBO> LoadAll()  //из DataObj в BusinessObj
        {
            var specials = unitOfWork.Specialization.GetAll();
            var res = specials.AsEnumerable().Select(a => mapper.Map<SpecializationBO>(a)).ToList();
            //res.ForEach(r => System.Diagnostics.Debug.WriteLine(r.Login));
            return res;
        }

        public void Load(int id)
        {
            var specials = unitOfWork.Specialization.GetById(id);
            mapper.Map(specials, this);
        }
        public void Save(SpecializationBO specialBO)
        {
            var special = mapper.Map<Specialization>(specialBO);
            if (specialBO.Id == 0) {
                Add(special);
            }
            else {
                Update(special);
            }
            unitOfWork.Specialization.Save();
        }
        private void Add(Specialization special)
        {
            unitOfWork.Specialization.Create(special);
        }
        private void Update(Specialization special)
        {
            unitOfWork.Specialization.Update(special);
        }
        public void DeleteSave(SpecializationBO specialBO)
        {
            var special = mapper.Map<Specialization>(specialBO);
            unitOfWork.Specialization.Delete(special.Id);
            unitOfWork.Specialization.Save();
        }
    }
}
