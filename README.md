# ModVault 🛡️

> **⚠️ Status:** This project is on indefinite pause and may not be resumed.

**ModVault** is a modern, lightweight desktop Mod Manager built with C# and WPF (.NET 8/9).

The goal of this project was to provide a clean and straightforward way to scan, manage, enable, and disable game mods without permanent file loss.

---

## 🛠️ Tech Stack & Learning Journey

* **Language:** C# 12/13
* **Framework:** WPF (.NET 8 / .NET 9)
* **Architecture:** Modular separation (Models / Services / UI)
* **IDE:** Visual Studio Community

> 💡 **Note on Development:**  
> This project was built as a hands-on learning exercise to explore C# and WPF desktop architecture with guidance from Gemini (AI). Code structure, commits, and comments were intentionally kept in English as part of best development practices.

---

## ✨ Features Implemented

- [x] Directory and standalone file scanning (`.asi`, `.dll`, `.pak`, `.zip`)
- [x] Toggle mods on/off using safe `.disabled` renaming
- [x] Local configuration persistence (saving the game directory)
- [x] Safe file deletion (moving items to the Windows Recycle Bin)
- [x] Drag-and-drop mod installation / extraction

---

## 📜 License

This project is open-source under the [MIT License](LICENSE.txt).
