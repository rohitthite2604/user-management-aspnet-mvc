# User Management System

## Overview
The **User Management System** is an ASP.NET MVC-based web application designed to manage user registration, login, and profile operations. It provides distinct functionalities for regular users and administrators to ensure secure access and proper privilege control. The system allows administrators to manage user accounts by activating or deactivating them, and users can update their profiles and view their details.

---

## Features

### User Functionality:
1. **Login Form:**
   - Users can log in using their **email or mobile number** along with a password.
   - Deactivated users will be restricted from logging in.

2. **Registration Form:**
   - Collects mandatory details such as:
     - Mobile Number
     - Username
     - Email
     - Password
     - Confirm Password
   - Supports optional profile image upload.

3. **Home Page:**
   - Displays all user details in a tabular format along with the profile picture (if provided).
   - Includes a navigation bar with an **Edit Profile** option.

4. **Edit Profile:**
   - Allows users to update their previously entered data, including the profile picture.

---

### Admin Functionality:
1. **Admin Login:**
   - Secure admin login functionality.
   - Displays a list of all registered users in a table format.

2. **User Management:**
   - Admin can **activate** or **deactivate** user accounts.
   - Deactivated users cannot log in.

---

## Technologies Used
- **Framework:** ASP.NET MVC (Not Core)
- **Frontend:** HTML5, CSS3, JavaScript, Bootstrap
- **Backend:** C#, Entity Framework
- **Database:** SQL Server
- **Authentication:** Session-based Authentication

---
