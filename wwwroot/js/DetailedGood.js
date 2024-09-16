    function category(obj) {
        let grandparent = obj.parentElement.parentElement;
        let grandchild = grandparent.querySelector(":scope > .subcategory-v");

        console.log(grandchild.style["display"]);
        if (grandchild.style["display"] == "block") {
            grandchild.style["display"] = "none";
            obj.style["transform"] = "";
        }
        else if (grandchild.style["display"] == "none" || grandchild.style["display"] == "") {
            grandchild.style["display"] = "block";
            obj.style["transform"] = "rotateX(180deg)";
        }


    }

    var product_img = document.getElementById("product-img");
    var small_img = document.getElementsByClassName("small-img");
    small_img[0].onclick= function(){
        product_img.src = small_img[0].src;

    }
    small_img[1].onclick= function(){
        product_img.src = small_img[1].src;
        
    }
    small_img[2].onclick= function(){
        product_img.src = small_img[2].src;
        
    }
    small_img[3].onclick= function(){
        product_img.src = small_img[3].src;
        
    }
