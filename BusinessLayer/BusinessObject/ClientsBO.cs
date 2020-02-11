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
        public int UserId { get; set; }
        virtual public UserBO User { get; set; }
        
        //public int? GraphicId { get; set; }

        //virtual public GraphTraningBO Graphic { get; set; }

        public ICollection<GraphTraningBO> GraphTraning { get; set; }
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
            return res;
        }
        public IEnumerable<ClientsBO> LoadAllWithInclude(params string[] parametrs)  //из DataObj в BusinessObj
        {
            var clients = unitOfWork.Clients.Include(parametrs);
            var res = clients.AsEnumerable().Select(c => mapper.Map<ClientsBO>(c)).ToList();
            return res;
        }
        public ClientsBO Load(int id)
        {
            var client = unitOfWork.Administration.GetById(id);
            return mapper.Map(client, this);
        }
        public void Save(ClientsBO clientBO)
        {
            var client = mapper.Map<Clients>(clientBO);
            if (clientBO.Id == 0) {
                Add(client);
            }
            else {
                Update(client);
            }
            unitOfWork.Clients.Save();
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
