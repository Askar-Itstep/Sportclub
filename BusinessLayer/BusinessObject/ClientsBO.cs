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
    public class ClientsBO: BaseBusinessObject
    {
        public int Id { get; set; }
        public int UserBOId { get; set; }
        virtual public UserBO UserBO { get; set; }
        
        public int? GraphicBOId { get; set; }

        virtual public GraphTraningBO GraphicBO { get; set; }

        //------------------------------------------------------------------------------------------
        readonly IUnityContainer unityContainer;
        public ClientsBO(IMapper mapper, UnitOfWork unitOfWork, IUnityContainer container)
            : base(mapper, unitOfWork)
        {
            unityContainer = container;
        }
        public IEnumerable<ClientsBO> LoadAll()  //из DataObj в BusinessObj
        {
            var clients = unitOfWork.Clients.GetAll();
            var res = clients.AsEnumerable().Select(c => mapper.Map<ClientsBO>(c)).ToList();
            //res.ForEach(r => System.Diagnostics.Debug.WriteLine(r.Login));
            return res;
        }

        public void Load(int id)
        {
            var client = unitOfWork.Administration.GetById(id);
            mapper.Map(client, this);
        }
        public void Save(ClientsBO adminBO)
        {
            var client = mapper.Map<Clients>(adminBO);
            if (adminBO.Id == 0) {
                Add(client);
            }
            else {
                Update(client);
            }
            unitOfWork.Administration.Save();
        }
        private void Add(Clients client)
        {
            unitOfWork.Clients.Create(client);
        }
        private void Update(Clients client)
        {
            unitOfWork.Clients.Update(client);
        }
        public void DeleteSave(ClientsBO clientBO)
        {
            var client = mapper.Map<Clients>(clientBO);
            unitOfWork.Clients.Delete(client.Id);
            unitOfWork.Clients.Save();
        }
    }
}
