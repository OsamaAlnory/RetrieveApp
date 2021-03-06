﻿using RetrieveApp.Database;
using RetrieveApp.Design;
using RetrieveApp.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RetrieveApp.Elements.Card
{
    public class LayoutButtons
    {
        int selected = 1;
        private const int MAX = 10;
        private int SLOT = 3;
        private StackLayout stk_btns;
        int page = 1;
        public static Color[] BTN_C = new Color[] { Color.FromHex("#b600ff"),
        Color.FromHex("#8300b7")};
        private StackLayout fl;
        private ScrollView sc;
        public List<Binary> visible = new List<Binary>();
        public string cardType = "default";


        public LayoutButtons(StackLayout stk_btns, StackLayout fl,
            ScrollView sc, string cardType)
        {
            this.stk_btns = stk_btns;
            this.fl = fl;
            this.sc = sc;
            this.cardType = cardType;
            foreach (View v in stk_btns.Children){
                Button b = v as Button;
                b.BackgroundColor = BTN_C[0];
                b.Clicked += ChangePage;
            }
        }

        public void AddItems(FilterState state)
        {
            fl.Children.Clear();
            for (int x = (page - 1) * MAX; x < page * MAX; x++)
            {
                if (visible.Count > x)
                {
                    fl.Children.Add(new ProductCard(visible[x], cardType, state));
                }
            }
            if(fl.Children.Count == 0)
            {
                fl.Children.Add(new NoItem());
            }
        }

        private void ReloadButtons()
        {
            List<Button> btns = new List<Button>();
            foreach (View v in stk_btns.Children)
            {
                if (v is Button)
                {
                    btns.Add(v as Button);
                }
            }
            if (page < SLOT + 1)
            {
                for (int x = 1; x < 6; x++)
                {
                    btns[x].Text = "" + x;
                }
            }
            else
            {
                for (int x = 1; x < 6; x++)
                {
                    btns[x].Text = "" + (page + x - SLOT);
                }
            }
            if (page < SLOT + 1)
            {
                selected = page;
            }
            else
            {
                selected = SLOT;
            }
            btns[selected].BackgroundColor = BTN_C[1];
            int pgCount = GetLastPage();
            if (pgCount < 1)
            {
                btns[selected].BackgroundColor = BTN_C[0];
                btns[selected].IsEnabled = false;
            }
            if (pgCount > 0)
            {
                btns[0].IsEnabled = true;
            }
            else
            {
                btns[0].IsEnabled = false;
            }
            btns[6].IsEnabled = true;
            if (page < 2)
            {
                btns[0].IsEnabled = false;
            }
            if (page >= pgCount)
            {
                btns[6].IsEnabled = false;
            }
            for (int x = 1; x < 6; x++)
            {
                btns[x].IsVisible = true;
                if (pgCount > 0)
                {
                    btns[x].IsEnabled = true;
                }
                if (x != selected)
                {
                    btns[x].BackgroundColor = BTN_C[0];
                }
                string b = btns[x].Text;
                if (b != ".")
                {
                    int i = int.Parse(b);
                    if (i > pgCount)
                    {
                        btns[x].Text = ".";
                        btns[x].IsEnabled = false;
                        btns[x].IsVisible = false;
                    }
                }
                else
                {
                    btns[x].IsEnabled = false;
                }
            }
            btns[selected].IsEnabled = false;
        }

        public void OpenPage(int p, FilterState state)
        {
            if(sc != null)
            {
                sc.ScrollToAsync(0, 0, false);
            }
            page = p;
            AddItems(state);
            ReloadButtons();
        }

        private void ChangePage(object o, object e)
        {
            string txt = (o as Button).Text;
            if (txt.Equals("."))
            {
                return;
            }
            if (txt.Equals("<<"))
            {
                OpenPage(1, MapPage.mapPage.current_state);
            }
            else if (txt.Equals(">>"))
            {
                OpenPage(GetLastPage(), MapPage.mapPage.current_state);
            }
            else
            {
                int n = int.Parse(txt);
                OpenPage(n, MapPage.mapPage.current_state);
            }
        }

        private int GetLastPage()
        {
            int pg = 0;
            int p = visible.Count;
            while (p >= MAX)
            {
                p -= MAX;
                pg++;
            }
            if (p > 0)
            {
                pg++;
            }
            return pg;
        }

    }
}
