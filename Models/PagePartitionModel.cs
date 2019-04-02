using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsApplication.Models
{
    public class PagePartitionModel
    {
        private int totalpage, currentpage = 1;
        private string action, controller;

        public int TotalPage
        {
            get
            {
                return this.totalpage;
            }
            set
            {
                this.totalpage = value;
            }
        }
        public int CurrentPage
        {
            get
            {
                return this.currentpage;
            }
            set
            {
                this.currentpage = value;
            }
        }
        public string Action
        {
            get
            {
                return this.action;
            }
            set
            {
                this.action = value;
            }
        }
        public string Controller
        {
            get
            {
                return this.controller;
            }
            set
            {
                this.controller = value;
            }
        }
        public PagePartitionModel(int cp, int tp)
        {
            this.totalpage = tp;
            this.currentpage = cp;
        }
        public PagePartitionModel(string action, string controller, int cp, int tp) : this(cp, tp)
        {
            this.controller = controller;
            this.action = action;
        }
    }
}