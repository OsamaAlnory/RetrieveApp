using RetrieveApp.Database;
using RetrieveApp.Elements.Card;
using RetrieveApp.Pages;
using System;
using System.Collections.Generic;
using System.Text;

namespace RetrieveApp.Events
{
    public class MapPageEvents
    {
        private LayoutButtons layout_b;
        private LayoutButtons la_2;
        private List<Products> vis1 = new List<Products>();
        private List<Products> vis_admin = new List<Products>();
        private string filter = "all";


        public MapPageEvents(LayoutButtons l1, LayoutButtons l2)
        {
            layout_b = l1;
            la_2 = l2;
        }

        public void AddItems()
        {
            layout_b.AddItems();
            la_2.AddItems();
        }

        public void ChangeFilter(string filter)
        {
            this.filter = filter;
            rel_admin(MapPage.mapPage.TXT());
            // Refresh
        }

        public void rel_guestB()
        {

        }

        public void rel(string f)
        {
            vis1.Clear();
            if (f != null && f != "")
            {
                f = f.ToLower();
                foreach (Products p in DBActions.products)
                {
                    Admins ad = DBActions._p(p);
                    if (ad.SName.ToLower().StartsWith(f))
                    {
                        vis1.Add(p);
                    }
                }
            }
            else
            {
                foreach (Products p in DBActions.products)
                {
                    vis1.Add(p);
                }
            }
            layout_b.visible = vis1;
            layout_b.OpenPage(1);
        }

        public void rel_admin(string f)
        {
            vis_admin.Clear();
            List<Products> list = DBActions._a((Admins)MapPage._g);
            if (f != null && f != "")
            {
                f = f.ToLower();
                if(filter == "all")
                {
                    foreach (Products p in list)
                    {
                        if (Check_A(p, f))
                        {
                            vis_admin.Add(p);
                        }
                    }
                } else if(filter == "book")
                {
                    
                }
            }
            else
            {
                foreach (Products p in list)
                {
                    vis_admin.Add(p);
                }
            }
            la_2.visible = vis_admin;
            la_2.OpenPage(1);
        }

        private bool Check_A(Products p, string text)
        {
            if (p.PName.ToLower().StartsWith(text))
            {
                return true;
            }
            return false;
        }

    }
}
