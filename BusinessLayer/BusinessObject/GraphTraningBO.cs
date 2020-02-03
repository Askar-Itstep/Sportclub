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
        #region Simple Fields
        public int Id { get; set; }

        public int? CoacheId { get; set; }
        public CoachesBO Coache { get; set; }

        public DayOfWeek DayOfWeek { get; set; }
        //--------------------------


        public List<ClientsBO> Clients { get; set; }


        [Required]
        [RegularExpression(@"(^(([0,1][0-9])|(2[0-3])):[0-5][0-9]$)", ErrorMessage = "Некорректное значение!")]
        public DateTime TimeBegin { get; set; }

        [Required]
        [RegularExpression(@"(^([0-1]\d|2[0-3])(:[0-5]\d){2}$) ", ErrorMessage = "Некорректное значение!")]
        public DateTime TimeEnd { get; set; }

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
            //res.ForEach(r => System.Diagnostics.Debug.WriteLine(r.Login));
            return res;
        }

        public void Load(int id)
        {
            var graphic = unitOfWork.GraphTranings.GetById(id);
            mapper.Map(graphic, this);
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
            unitOfWork.Users.Save();
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
