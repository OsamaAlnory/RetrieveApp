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
        private List<Binary> vis_admin = new List<Binary>();
        private string filter = "all";


        public MapPageEvents(LayoutButtons l1, LayoutButtons l2)
        {
            layout_b = l1;
            la_2 = l2;
        }

        public void AddItems()
        {
            layout_b.AddItems();
            //la_2.AddItems();
        }

        public void ChangeFilter(string filter)
        {
            this.filter = filter;
            rel(MapPage.mapPage.TXT());
            // Refresh
        }

        public void rel_guestB()
        {

        }

        public void rel(string f)
        {
            vis1.Clear();
            Account acc = MapPage._g;
            if(filter == "all")
            {
                layout_b.cardType = "default";
            } else if(filter == "b")
            {
                layout_b.cardType = "admin";
            }
            if (f != null && f != "")
            {
                f = f.ToLower();
                if(filter == "all")
                {
                    foreach (Products p in DBActions.products)
                    {
                        Admins ad = DBActions._p(p);
                        if (ad.SName.ToLower().StartsWith(f))
                        {
                            vis1.Add(p);
                        }
                    }
                } else if(filter == "b")
                {
                    if(acc is Admins)
                    {
                        var acc1 = acc as Admins;
                        var list = DBActions._a(acc1);
                        foreach (Products p in list)
                        {
                            if (p.PName.ToLower().StartsWith(f))
                            {
                                vis1.Add(p);
                            }
                        }
                    }
                }
            }
            else
            {
                if(filter == "all")
                {
                    foreach (Products p in DBActions.products)
                    {
                        vis1.Add(p);
                    }
                } else if(filter == "b")
                {
                    if(acc is Admins)
                    {
                        vis1 = DBActions._a(acc as Admins);
                    } else
                    {
                        vis1 = DBActions.GetProducts(acc as Guests);
                    }
                }
            }
            var tst = new List<Binary>();
            foreach(Products p in vis1)
            {
                tst.Add(new Binary { PRODUCT=p});
            }
            layout_b.visible = tst;
            layout_b.OpenPage(1);
        }

        public void rel_admin(string f)
        {
            vis_admin.Clear();
            List<Products> list = DBActions._a((Admins)MapPage._g);
            if (f != null && f != "")
            {
                f = f.ToLower();
                foreach (Products p in list)
                {
                    if (Check_A(p, f))
                    {
                        //vis_admin.Add(p);
                    }
                }
            }
            else
            {
                foreach(Guests g in DBActions.guests)
                {
                    var l1 = DBActions.GetProducts(g);
                    foreach(Products p in l1)
                    {
                        if (list.Contains(p))
                        {
                            vis_admin.Add(new Binary { OWNER=g, PRODUCT=p});
                        }
                    }
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
