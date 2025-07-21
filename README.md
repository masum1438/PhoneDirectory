#  Phone Directory API

This is a Web API project for managing contacts with Admin and Client roles. It supports individual and bulk contact operations, Excel import/export, and an auto-delete feature via background services.

---

##  Auth API

- **POST `/api/Auth/register`**  
  Registers a new user with email and password. Returns JWT token on success.

- **POST `/api/Auth/create-user`**  
  Allows Admin to create users with specific roles (Admin/Client). Used for backend user management.

- **POST `/api/Auth/login`**  
  Authenticates a user and returns a JWT token if credentials are valid.

- **POST `/api/Auth/logout`**  
  Invalidates the user's session or token on the client side (typically clears the cookie).

---

## 📇 Contact API

- **GET `/api/Contact`**  
  Returns a list of all contacts.  
  
  

- **POST `/api/Contact`**  
  Creates a new contact. Requires valid fields like Name, Email, Group, etc.

- **GET `/api/Contact/{id}`**  
  Fetches a specific contact by its ID.  
  

- **PUT `/api/Contact/{id}`**  
  Updates an existing contact's full details by ID.  
 
  

- **DELETE `/api/Contact/{id}`**  
  Deletes a single contact permanently by ID.

- **PATCH `/api/Contact/{id}/disable`**  
  Disables (soft-deletes) a contact. It marks the contact as inactive.

---

##  Bulk Contact Operations

- **POST `/api/Contact/bulk`**  
  Uploads and inserts multiple contacts (e.g., from Excel) one by one via background service.

- **DELETE `/api/Contact/bulk`**  
  Deletes multiple contacts one by one using their IDs.

- **PATCH `/api/Contact/bulk/disable`**  
  Disables multiple contacts (marks them inactive) one by one.

---

## File Import/Export

- **POST `/api/Contact/import`**  
  Imports contacts from an uploaded Excel file and processes them asynchronously.

- **GET `/api/Contact/export`**  
  Exports all contacts as an Excel file for download.

---

## Auto Delete Feature

- **POST `/api/Contact/bulk-delete-auto`**  
  Triggers auto-delete of a defined number of contacts according to admin-defined settings.

- **POST `/api/Contact/toggle/{isEnabled}`**  
  Enables or disables the auto-delete feature (controlled by Admin).

---

##  User Roles

- **Admin:**  
  Full control — can view, create, update, disable, delete any contact. Can also manage users and toggle auto-delete settings.

- **Client:**  
  Limited access — can view  contact list and contact details.

---
##  Adding project font-end image
<img width="1248" height="531" alt="fnt-login" src="https://github.com/user-attachments/assets/706ceeba-c94b-444b-ad57-12386c28b305" />
<img width="1211" height="557" alt="fnt-register" src="https://github.com/user-attachments/assets/f08e8249-ce7e-437d-b359-e6796f0f6d02" />

---
- **Admin view:**
<img width="1287" height="638" alt="fend-contact-page" src="https://github.com/user-attachments/assets/f462a2fd-6615-4583-8999-2547c3c3bb2a" />
<img width="1274" height="516" alt="fnt-auto-delete" src="https://github.com/user-attachments/assets/75f6edf6-3e17-4d36-953b-7690fa649afe" />
<img width="1269" height="511" alt="fnt-user-manage" src="https://github.com/user-attachments/assets/55a0862b-4961-4ba5-b233-8cdd8d9c756b" />

---

- **Client view:**
<img width="1241" height="613" alt="client-view" src="https://github.com/user-attachments/assets/f77d0f48-9d19-462d-86cc-abe1f3412746" />

---
##Main Api
<img width="1014" height="409" alt="image" src="https://github.com/user-attachments/assets/2f97a90a-cd8a-41bc-9d3c-422cb5fdfea3" />
<img width="1014" height="518" alt="image" src="https://github.com/user-attachments/assets/5bff817c-c506-4878-90e7-c2a0ecc93c32" />


##  Adding project swagger-api image

<img width="732" height="628" alt="login" src="https://github.com/user-attachments/assets/c1592da2-7c43-435c-8a65-8017b190d729" />

---
<img width="496" height="454" alt="regis" src="https://github.com/user-attachments/assets/1d5fde01-25d9-4228-ba85-fbb7b78b1fe0" />

---
<img width="737" height="640" alt="create-user" src="https://github.com/user-attachments/assets/d433c5db-fe65-41af-8329-49438115fbd7" />

---
<img width="747" height="462" alt="logout" src="https://github.com/user-attachments/assets/63c52ed6-1e58-4827-9888-13c48862efd7" />

---

<img width="751" height="645" alt="get-api-contact" src="https://github.com/user-attachments/assets/1db2a1d1-8949-443a-90ff-506b662762ae" />

---
<img width="745" height="569" alt="get-contact-id" src="https://github.com/user-attachments/assets/018f0469-8b4d-456a-bc9e-255a91ec941f" />

---
<img width="744" height="640" alt="post-api-contact" src="https://github.com/user-attachments/assets/6064c75f-459f-4929-ac18-09a67c9633dc" />

---
<img width="726" height="441" alt="patch-contact-disable" src="https://github.com/user-attachments/assets/9a810091-2f53-44a6-9593-9412ca39ef97" />

---
<img width="735" height="607" alt="put-api-contact" src="https://github.com/user-attachments/assets/3d123141-c62d-4b93-8f64-a65a9da43e90" />

---
<img width="728" height="436" alt="contact-delete" src="https://github.com/user-attachments/assets/6e759379-a2f0-4556-a6a2-d9f3105fee67" />

---

<img width="736" height="634" alt="bulk-insert" src="https://github.com/user-attachments/assets/4399c1e9-4112-4d33-9977-08300f946923" />

---
<img width="727" height="644" alt="bulk-delete-array" src="https://github.com/user-attachments/assets/d6995fc1-f004-44fa-a615-3fd5d29c588c" />

---
<img width="724" height="642" alt="bulk-disable-array" src="https://github.com/user-attachments/assets/6d89cc93-95d1-43f5-abbc-c7bee81217da" />

---

<img width="739" height="572" alt="bulk-auto-delete" src="https://github.com/user-attachments/assets/a8f49b5c-a1a3-485b-a8b9-df8609105892" />

---
<img width="728" height="503" alt="auto-delete-enable" src="https://github.com/user-attachments/assets/9955fe90-5467-481f-8faf-f542bf9b7c7a" />

---
<img width="743" height="566" alt="excell-import" src="https://github.com/user-attachments/assets/515ecfe6-d368-44eb-bc97-9f942c516388" />

---
<img width="717" height="435" alt="download-excell" src="https://github.com/user-attachments/assets/e50686d8-d331-402e-a76c-40449b19d86a" />





