﻿
    function account_anchor_click(obj) {
        let elem = obj.parentElement.querySelector("div");
    console.log(elem);
    console.log(elem.style["display"] );
    if (elem.style["display"] == "none" || elem.style["display"] == "") {
        elem.style["display"] = "block";
        }
    else if (elem.style["display"] == "block") {
        elem.style["display"] = "none";
        }
    }

    function category_icon_click(obj) {
        let grandparent = obj.parentElement.parentElement;
        let grandchild = grandparent.querySelector(":scope > .subcategory-v");

    if (grandchild.style["display"] == "block") {
        grandchild.style["display"] = "none";
    obj.style["transform"] = "";
        }
    else if (grandchild.style["display"] == "none" || grandchild.style["display"] == "") {
        grandchild.style["display"] = "block";
    obj.style["transform"] = "rotateX(180deg)";
        }

    }

    $(function () {
        $('#Search-input').keyup(function (event) {
            if (event.keyCode === 13) {
                $('#Search-form').submit();
            }
        });
    });

    let form = document.getElementById("Search-form");
    let a = document.getElementById("input-anchor");
    console.log(a);
    a.addEventListener("click", function () {
        form.submit();
    });

let input_block = document.getElementById("Search-input");
let search_button = document.getElementById("Search-button");
input_block.addEventListener("focus", function () {
    input_block.style.outline = "none";
    input_block.style.border = "2px solid blue";
    input_block.style.borderRight = "none";
    search_button.style.border = "2px solid blue";
    search_button.style.borderLeft = "none";

    });


    let dropdown_wrapper = document.getElementById("dropdown-wrapper");
    let dropdown_open = document.getElementById("dropdown-open");
    let dropdown_close_wrapper = document.getElementById("dropdown-close-wrapper");
    let dropdown_close = document.getElementById("dropdown-close"); 

    document.getElementById("dropdown-open").addEventListener("click", function () {
       
        dropdown_wrapper.style.display = "block";
        dropdown_close_wrapper.style.width = "32px ";
        dropdown_close.style.display = "block";
        dropdown_open.style.display = "none";
        
    });

    
document.getElementById("dropdown-close").addEventListener("click", function () {
        dropdown_open.style.display = "block";
    dropdown_wrapper.style.display = "none";
    dropdown_close.style.display = "none";
        dropdown_close_wrapper.style.width = "0px ";
    });


