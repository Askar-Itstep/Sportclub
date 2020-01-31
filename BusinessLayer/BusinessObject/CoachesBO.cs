﻿using AutoMapper;
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
    public class CoachesBO: BaseBusinessObject
    {
        public int Id { get; set; }
        
        public int UserBOId { get; set; }
        public UserBO UserBO { get; set; }
        public StatusCoach Status { get; set; }
        
        public int SpecializationBOId { get; set; }

        virtual public SpecializationBO SpecializationBO { get; set; }
        public double TimeWork { get; set; }


        public enum StatusCoach
        {
            COACHE, HEAD_COACHE_HALL, TOP_COACHE
        }
        //------------------------------------------------------------------------------------
        readonly IUnityContainer unityContainer;
        public CoachesBO(IMapper mapper, UnitOfWork unitOfWork, IUnityContainer container)
            : base(mapper, unitOfWork)
        {
            unityContainer = container;
        }
        public IEnumerable<CoachesBO> LoadAll()  //из DataObj в BusinessObj
        {
            var managers = unitOfWork.Coaches.GetAll();
            var res = managers.AsEnumerable().Select(a => mapper.Map<CoachesBO>(a)).ToList();
            //res.ForEach(r => System.Diagnostics.Debug.WriteLine(r.Login));
            return res;
        }

        public void Load(int id)
        {
            var managers = unitOfWork.Coaches.GetById(id);
            mapper.Map(managers, this);
        }
        public void Save(CoachesBO coacheBO)
        {
            var coache = mapper.Map<Coaches>(coacheBO);
            if (coacheBO.Id == 0) {
                Add(coache);
            }
            else {
                Update(coache);
            }
            unitOfWork.Coaches.Save();
        }
        private void Add(Coaches coache)
        {
            unitOfWork.Coaches.Create(coache);
        }
        private void Update(Coaches coache)
        {
            unitOfWork.Coaches.Update(coache);
        }
        public void DeleteSave(CoachesBO coacheBO)
        {
            var coache = mapper.Map<Coaches>(coacheBO);
            unitOfWork.Coaches.Delete(coache.Id);
            unitOfWork.Coaches.Save();
        }
    }
}
