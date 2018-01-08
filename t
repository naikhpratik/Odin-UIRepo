[1mdiff --git a/Odin.Data/Persistence/OrdersRepository.cs b/Odin.Data/Persistence/OrdersRepository.cs[m
[1mindex 99ecbc8..07d5fff 100644[m
[1m--- a/Odin.Data/Persistence/OrdersRepository.cs[m
[1m+++ b/Odin.Data/Persistence/OrdersRepository.cs[m
[36m@@ -106,7 +106,7 @@[m [mnamespace Odin.Data.Persistence[m
         public Order GetOrderFor(string userId, string orderId)[m
         {[m
             return _context.Orders[m
[31m-                .Where(o => o.Id == orderId && (o.ConsultantId == userId || o.TransfereeId == userId))[m
[32m+[m[32m                .Where(o => o.Id == orderId && (o.ConsultantId == userId || o.TransfereeId == userId || o.ProgramManagerId == userId))[m
                 .Include(o => o.Services)[m
                 .Include(o => o.HomeFinding)[m
                 .Include(o => o.Services.Select(s => s.ServiceType))[m
[1mdiff --git a/Odin/Odin.csproj b/Odin/Odin.csproj[m
[1mindex d8a6136..3d8a334 100644[m
[1m--- a/Odin/Odin.csproj[m
[1m+++ b/Odin/Odin.csproj[m
[36m@@ -19,7 +19,7 @@[m
     <MvcBuildViews>false</MvcBuildViews>[m
     <UseIISExpress>true</UseIISExpress>[m
     <Use64BitIISExpress />[m
[31m-    <IISExpressSSLPort>44303</IISExpressSSLPort>[m
[32m+[m[32m    <IISExpressSSLPort>44394</IISExpressSSLPort>[m
     <IISExpressAnonymousAuthentication />[m
     <IISExpressWindowsAuthentication />[m
     <IISExpressUseClassicPipelineMode />[m
[36m@@ -311,6 +311,7 @@[m
     <Compile Include="Controllers\Api\ConsultantsController.cs" />[m
     <Compile Include="Controllers\Api\AppointmentController.cs" />[m
     <Compile Include="Controllers\Api\MessageController.cs" />[m
[32m+[m[32m    <Compile Include="Controllers\Api\TransfereesController.cs" />[m
     <Compile Include="Controllers\AppointmentController.cs" />[m
     <Compile Include="Controllers\EmailController.cs" />[m
     <Compile Include="Controllers\HelpController.cs" />[m
[1mdiff --git a/Odin/Scripts/app/views/transferee-intake.js b/Odin/Scripts/app/views/transferee-intake.js[m
[1mindex de72656..8b90efd 100644[m
[1m--- a/Odin/Scripts/app/views/transferee-intake.js[m
[1m+++ b/Odin/Scripts/app/views/transferee-intake.js[m
[36m@@ -26,7 +26,7 @@[m
     }[m
 [m
     var inviteTransferee = function(orderId, success, fail) {[m
[31m-        var url = route + "/invite";[m
[32m+[m[32m        var url = route + "/invite/" + orderId;[m
         $.post(url).done(success).fail(fail);[m
     }[m
 [m
[1mdiff --git a/Odin/Views/Orders/Partials/_Itinerary.cshtml b/Odin/Views/Orders/Partials/_Itinerary.cshtml[m
[1mindex a1716d8..9b0824c 100644[m
[1m--- a/Odin/Views/Orders/Partials/_Itinerary.cshtml[m
[1m+++ b/Odin/Views/Orders/Partials/_Itinerary.cshtml[m
[36m@@ -123,7 +123,7 @@[m
     </ul>[m
     <div class="modal fade appointmentModal" id="modalForm" tabindex="-100" role="dialog" data-backdrop="false" aria-labelledby="appointmentModal">[m
         <div class="modal-dialog" role="document" id="appointment" style="z-index: 2147483647;">[m
[31m-            <div class="modal-content id="appointmentModalLabel"">[m
[32m+[m[32m            <div class="modal-content" id="appointmentModalLabel">[m
                 <div class="modal-header row">[m
                     <h5 class="modal-title"></h5>[m
                     <button type="button" class="close" data-dismiss="modal" aria-label="Close">[m
