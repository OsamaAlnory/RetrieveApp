using RetrieveApp.Database;
using RetrieveApp.Elements;
using RetrieveApp.Elements.Card;
using RetrieveApp.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RetrieveApp.Events
{
    public class MapPageEvents
    {
        private LayoutButtons layout_b;
        private LayoutButtons la_2;
        private List<Binary> vis1 = new List<Binary>();
        private List<Binary> vis_admin = new List<Binary>();
        public FilterState filter = FilterState.ALL;


        public MapPageEvents(LayoutButtons l1, LayoutButtons l2)
        {
            layout_b = l1;
            la_2 = l2;
        }

        public void AddItems()
        {
            layout_b.AddItems();
            if(MapPage._g is Admins)
            {
                la_2.AddItems();
            }
        }

        public void ChangeFilter(FilterState filter)
        {
            this.filter = filter;
            MapPage.mapPage.current_state = filter;
            rel(MapPage.mapPage.TXT());
        }

        public void rel(string f)
        {
            vis1.Clear();
            Account acc = MapPage._g;
            if(filter == FilterState.ALL)
            {
                layout_b.cardType = "default";
            } else if(filter == FilterState.B)
            {
                if(acc is Admins)
                {
                    layout_b.cardType = "admin";
                } else if(acc is Guests)
                {
                    layout_b.cardType = "booked";
                }
            }
            if (f != null && f != "")
            {
                f = f.ToLower();
                if(filter == FilterState.ALL)
                {
                    foreach (Products p in DBActions.products)
                    {
                        if(p.Quantity > 0)
                        {
                            Admins ad = DBActions._p(p);
                            if(ad != null)
                            {
                                if (ad.SName.ToLower().StartsWith(f))
                                {
                                    vis1.Add(new Binary { PRODUCT = p, ADMIN = ad });
                                }
                            }
                        }
                    }
                } else if(filter == FilterState.B)
                {
                    if(acc is Admins)
                    {
                        var acc1 = acc as Admins;
                        var list = DBActions._a(acc1);
                        foreach (Products p in list)
                        {
                            if (p.PName.ToLower().StartsWith(f))
                            {
                                vis1.Add(new Binary { PRODUCT = p });
                            }
                        }
                    }
                }
            }
            else
            {
                if(filter == FilterState.ALL)
                {
                    foreach (Products p in DBActions.products)
                    {
                        if(p.Quantity > 0)
                        {
                            vis1.Add(new Binary { PRODUCT = p });
                        }
                    }
                } else if(filter == FilterState.B)
                {
                    if(acc is Admins)
                    {
                        var ap = DBActions._a(acc as Admins);
                        foreach(Products prd in ap)
                        {
                            vis1.Add(new Binary { PRODUCT = prd});
                        }
                    } else
                    {
                        var ap = DBActions.GetProducts(acc as Guests);
                        var ap1 = DBActions.GetQuantities(acc as Guests);
                        for(int x = 0; x < ap.Count; x++)
                        {
                            vis1.Add(new Binary { PRODUCT = ap[x],
                                QUANTITY = ap1[x], OWNER = acc as Guests});
                        }
                    }
                }
            }
            layout_b.visible = vis1;
            layout_b.OpenPage(1);
        }

        public void rel_admin(string f)
        {
            la_2.cardType = "user";
            vis_admin.Clear();
            List<Products> list = DBActions._a((Admins)MapPage._g);
            if (f != null && f != "")
            {
                f = f.ToLower();
                foreach (Guests g in DBActions.guests)
                {
                    if (g.Name.ToLower().StartsWith(f))
                    {
                        var l1 = DBActions.GetProducts(g);
                        foreach (Products p in l1)
                        {
                            if (list.Contains(p))
                            {
                                vis_admin.Add(new Binary { OWNER = g, PRODUCT = p,
                                QUANTITY = DBActions.GetQuantity(g, p)});
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (Guests g in DBActions.guests)
                {
                    var l1 = DBActions.GetProducts(g);
                    foreach (Products p in l1)
                    {
                        if (list.Contains(p))
                        {
                            vis_admin.Add(new Binary { OWNER = g, PRODUCT = p,
                                QUANTITY = DBActions.GetQuantity(g, p)
                            });
                        }
                    }
                }
            }
            la_2.visible = vis_admin;
            la_2.OpenPage(1);
        }

    }
}
