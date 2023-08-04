using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using ShopProject.Data;
using ShopProject.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ShopProject.TagHelpers
{
    public class CategoriesTagHelper : TagHelper
    {
        public Orientation orientation { get; set; }

        [HtmlAttributeName("Mobile")]
        public bool IsForMobile { get; set; } = false;
        UnitOfWork unit;
        IServiceScopeFactory service;
        delegate void GetHtmlContent(int id, ref string output);


        string category_class = null;
        string subcategory_class = null;
        string caret_direction = null;
        [HtmlAttributeName("OnClick")]
        string icon_onclick { get; set; } = "onclick =\"category_icon_click(this)\"";

        public CategoriesTagHelper(UnitOfWork unit, IServiceScopeFactory serviceScopeFactory)
        {
            this.unit = unit;
            this.service = serviceScopeFactory;
        }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {   
            output.TagName = null;
            output.Content.SetHtmlContent(GetCategories());
        }

        public string GetCategories()
        {
            GetHtmlContent getHtmlContent = MakeCategoriesHtml;
            if (orientation == Orientation.Horizontal)
            {
                category_class = "category";
                subcategory_class = "subcategory";
                caret_direction = "right";
            }
            else
            {
                category_class = "category-v";
                subcategory_class = "subcategory-v";
                caret_direction = "down";
            }

            if(IsForMobile== true)
            {
                getHtmlContent = MakeCategoriesHtmlMobile;
            }
            
            string output = null;
            using ( var scope = service.CreateScope()){
                var serv = scope.ServiceProvider.GetRequiredService<UnitOfWork>();
                var categories = serv.CategoryRepository.GetByParentCategory(0);

                foreach (var category in categories.ToList())
                {
                    string temp = null;
                    getHtmlContent.Invoke(category.Id, ref temp);
                    output += temp;
                }
                return output;
            }
        }

       

        public  void MakeCategoriesHtml(int id, ref string childoutput)
        {
            var subcategories = unit.CategoryRepository.GetByParentCategory(id);
            if (subcategories.Count >0)
            {
                childoutput +=
                    "<div class=\"" +category_class+ "\">" +
                        "<a class=\"nav-link text-dark\" href=\"/Good?category_id="+ id.ToString()+ "\"> " +
                            "Category "+  id.ToString() + " " +
                            "<i " + icon_onclick+" class=\"fa fa-caret-"+ caret_direction +"\"></i> " +
                         "</a> " +
                         "<div class=\"" + subcategory_class + "\">";

                foreach (var subcategory in subcategories.ToList())
                {
                    MakeCategoriesHtml(subcategory.Id,ref childoutput);
                }
                // После добавления данных последнего дочернего закрываем <div class ="category "> и <div class = "subcategoroy">
                childoutput += " </div> </div> ";
            }
            if (subcategories.Count ==0)
            {
                {//Когда уже нет дочерних, то добавляет свои данные собственно
                    childoutput += 
                        "<a class=\"nav-link text-dark\" href=\"/Good?category_id=" + id.ToString() + "\">" +
                            "Category"+ id.ToString() + 
                        "</a>";
                }

            }
                    }

        public  void MakeCategoriesHtmlMobile(int id, ref string childoutput)
        {
            var subcategories = unit.CategoryRepository.GetByParentCategory(id);
            if (subcategories.Count > 0)
            {
                childoutput +=
                    "<div class=\"" + category_class + "\">" +
                        "<div class=\"anchor-wrapper\">"+
                            "<a class=\" \" href=\"/Good?category_id="+ id.ToString() +"\"> " +
                                "Category " + id.ToString() + 
                            "</a>"+ 
                            "<i "+icon_onclick + " class=\"fa fa-caret-" + caret_direction + "\"></i> " +
                        "</div>" +
                        "<div class=\"" + subcategory_class + "\">";

                    

                foreach (var subcategory in subcategories.ToList())
                {
                    MakeCategoriesHtmlMobile(subcategory.Id, ref childoutput);
                }

                childoutput += " </div> </div> ";
            }
            if (subcategories.Count == 0)
            {
                {
                    childoutput += 
                     "<div class=\"anchor-wrapper\">"+
                        "<a class=\"nav-link text-dark\" href=\"/Good?category_id=" + id.ToString() + "\">" +
                            "Category" + id.ToString() + 
                        "</a>"
                     +"</div>";
                }

            }



        }
    }
    public enum Orientation
    {
       Horizontal,
       Vertical
    }
}
