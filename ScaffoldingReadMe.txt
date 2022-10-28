Scaffolding has generated all the files and added the required dependencies.

However the Application's Startup code may required additional changes for things to work end to end.
Add the following code to the Configure method in your Application's Startup class if not already done:

        app.UseMvc(routes =>
        {
          routes.MapRoute(
            name : "areas",
            template : "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );
        });
       
       *****B. Associating controllers with an area
To connect a controller to an area, you can use the Area attribute

When using the Area attribute, you must use the areaName that you specified in the startup.cs file.

Example using the Admin areaName:

      namespace GuitarShop.Areas.Admin.Controllers {
     		 [Area("Admin")]
      	 public class HomeController : Controller {
     				 public IActionResult Index() {
      				return View();  // maps to /Areas/Admin/Views/Home/Index.cshtml 
					}
			 }
		 }

         YOU HAVE TO HAVE AREAS/ADMIN IN YOUR AREA ADMIN VIEWS IN ORDER FOR THE ROUTING TO WORK.

        *** If you want to specify an area in the route attribute, you need to use [area] in the same way you used [controller] and [action]

Example:

      [Area("Admin")]
      public class ProductController : Controller {
		   [Route("[area]/[controller]s/{id?}")] 
			public IActionResult List(string id = "All")  {...};
    		public IActionResult Add() {...}; 
			public IActionResult Update(int id) {...}; 
 			public IActionResult Delete(int id) {...};
		}