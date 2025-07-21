# 📞 Phone Directory API

This is a .NET 8 Web API project for managing contacts with Admin and Client roles. It supports individual and bulk contact operations, Excel import/export, and an auto-delete feature via background services.

---

## 🔐 Auth API

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
  ✅ Admin sees all contacts.  
  ✅ Client sees only their own contacts.

- **POST `/api/Contact`**  
  Creates a new contact. Requires valid fields like Name, Email, Group, etc.

- **GET `/api/Contact/{id}`**  
  Fetches a specific contact by its ID.  
  ✅ Clients can only view their **own** contact details.

- **PUT `/api/Contact/{id}`**  
  Updates an existing contact's full details by ID.  
  ✅ Admins can update any.  
  ✅ Clients can update their own (if permitted).

- **DELETE `/api/Contact/{id}`**  
  Deletes a single contact permanently by ID.

- **PATCH `/api/Contact/{id}/disable`**  
  Disables (soft-deletes) a contact. It marks the contact as inactive.

---

## 📁 Bulk Contact Operations

- **POST `/api/Contact/bulk`**  
  Uploads and inserts multiple contacts (e.g., from Excel) one by one via background service.

- **DELETE `/api/Contact/bulk`**  
  Deletes multiple contacts one by one using their IDs.

- **PATCH `/api/Contact/bulk/disable`**  
  Disables multiple contacts (marks them inactive) one by one.

---

## 📤📥 File Import/Export

- **POST `/api/Contact/import`**  
  Imports contacts from an uploaded Excel file and processes them asynchronously.

- **GET `/api/Contact/export`**  
  Exports all contacts as an Excel file for download.

---

## 🔄 Auto Delete Feature

- **POST `/api/Contact/bulk-delete-auto`**  
  Triggers auto-delete of a defined number of contacts according to admin-defined settings.

- **POST `/api/Contact/toggle/{isEnabled}`**  
  Enables or disables the auto-delete feature (controlled by Admin).

---

## 👥 User Roles

- **Admin:**  
  Full control — can view, create, update, disable, delete any contact. Can also manage users and toggle auto-delete settings.

- **Client:**  
  Limited access — can view  contact list and contact details.

---

## 📷 Adding Image in README

To include an image (e.g., UI preview), use the following Markdown syntax:

```markdown
![Alt Text](<img width="728" height="503" alt="Image" src="https://github.com/user-attachments/assets/fd8f877a-45d7-4edf-9695-72deba061e42" />)
