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
    public class GraphTraningBO: BaseBusinessObject
    {
        public int Id { get; set; }

        public int? CoacheId { get; set; }
        public CoachesBO Coache { get; set; }
        public int? GymsId { get; set; }
        public GymsBO Gyms { get; set; }
        public DayOfWeek DayOfWeek { get; set; }

        //----------------------------------------

        //public List<ClientsBO> Clients { get; set; }
        public ICollection<ClientsBO> Clients { get; set; }

        [Required]
        public DateTime TimeBegin { get; set; }

        [Required]
        public DateTime TimeEnd { get; set; }

        #region Del Fields
        //public GraphTraningBO()
        //{
        //    this.ClientsBO = new List<ClientsBO>();
        //}
        //public IEnumerator<ClientsBO> GetEnumerator()
        //{
        //    for (int i = 0; i < ClientsBO.Count; i++)
        //        yield return ClientsBO[i];
        //}
        #endregion
        readonly IUnityContainer unityContainer;
        public GraphTraningBO(IMapper mapper, UnitOfWork unitOfWork, IUnityContainer container)
            : base(mapper, unitOfWork)
        {
            unityContainer = container;
        }
        public IEnumerable<GraphTraningBO> LoadAll()  //из DataObj в BusinessObj
        {
            var graphics = unitOfWork.GraphTranings.GetAll();
            var res = graphics.AsEnumerable().Select(g => mapper.Map<GraphTraningBO>(g)).ToList();
            return res;
        }

        public IEnumerable<GraphTraningBO> LoadAllWithInclude(params string[] values)
        {
            var graphics = unitOfWork.GraphTranings.Include(values).ToList();
            var graphicsBO = graphics.Select(g => mapper.Map<GraphTraningBO>(g)).ToList();
            
            return graphicsBO;
        }
        public GraphTraningBO Load(int id)
        {
            var graphic = unitOfWork.GraphTranings.GetById(id);
            return mapper.Map(graphic, this);
        }
        public void Save(GraphTraningBO graphicBO)
        {
            var graphic = mapper.Map<GraphTraning>(graphicBO);
            if (graphicBO.Id == 0) {
                Add(graphic);
            }
            else {
                Update(graphic);
            }
            unitOfWork.GraphTranings.Save();
        }
        private void Add(GraphTraning graphic)
        {
            unitOfWork.GraphTranings.Create(graphic);
        }
        private void Update(GraphTraning graphic)
        {
            unitOfWork.GraphTranings.Update(graphic);
        }
        public void DeleteSave(GraphTraningBO graphicBO)
        {
            var user = mapper.Map<GraphTraning>(graphicBO);
            unitOfWork.GraphTranings.Delete(graphicBO.Id);
            unitOfWork.GraphTranings.Save();
        }
        
    }
}
