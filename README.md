E-Commerce Order Management System
the project consists of 3 layers(Api,data.access,Models)


- u cant check categories (admin account needed)  .

customer section:
_Register Customer Account
- login with customer account and authorize customer roles with jwt token
- check the products btw u can check that even without login as a customer 
- order unavailable product(out of stock) ex :product id =6
- order available  product ex : product id =1 ,2 with quantities =10,15
- see my customer order summary and status (pending=default)note: the products are summarized in one order 
- logout and login with admin account using the jwt token

admin section:

-- 

-admin authorizations :
1- view all categories .
2- view all orders done .
3- change the order status from pending to cancelled .


