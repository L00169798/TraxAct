using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraxAct.Models;

namespace TraxAct.ViewModels
{
    public class EditEventMessage
    {
        public Event EventItem { get; set; }

        public EditEventMessage(Event eventItem)
        {
            EventItem = eventItem;
        }
    }
}
