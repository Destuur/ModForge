# KCD2 ModForge

> ?? **Notice:** This application is currently in active development and not yet available to the public. A first public release is planned once core functionality is stable.

**KCD2 ModForge** is a WPF application with a Blazor/MudBlazor frontend designed to help modders of *Kingdom Come: Deliverance 2* easily read, edit, and export XML-based game files into fully playable mods.

---

## Features

- Import, edit, and create Perks, Buffs, Debuffs, Localizations, and more  
- Automatic generation of mod folder structure including official `mod.manifest` file compatible with KCD2 modding tools  
- Export mods as `.pak` files ready to be used in the game  
- Project management with recent mods list for quick access  
- Seamless workflow from creating a new mod to editing elements and exporting  
- Future support for additional file types like Items, InventoryPresets, and STORM files

---

## Upcoming Improvements

### Settings & Personalization
- Save the game installation path and username to auto-fill mod metadata during creation  
- Support for language settings and other user preferences

### Caching & Performance
- Current behavior: XML files are read on demand  
- Planned: Load all data at startup, process it, and cache it locally in a `.json` file for faster access during app usage

### Multilingual Support
- User interface available in multiple languages with AI-assisted translations

### Improved Workflow
- After creating the `mod.manifest`, users are taken directly to the Perks overview  
- Option to create a new empty element or edit an existing one when opening the mod

### Code Quality & Refactoring
- Ongoing refactoring to improve structure, maintainability, and separation of concerns  
- Cleaner architecture separating UI, business logic, and data access layers

### Extended Functionality
- Adding and editing Buffs and Debuffs support

---

## How It Works

1. Open an existing mod or create a new one by entering required metadata  
2. The app generates the mod folder structure and official manifest file  
3. Create or edit mod elements like Perks, Buffs, Localizations, etc.  
4. Export your mod as a `.pak` file compatible with *Kingdom Come: Deliverance 2*  
5. Launch the game or add the mod ID to your `mod_order.txt` to activate

---

## Contribution & Feedback

Contributions, bug reports, and feature requests are very welcome!  
Feel free to open issues or pull requests on GitHub.

---

## License

This project is licensed under the [MIT License](Licence).
You are free to use, modify, and distribute this software, provided that the original copyright and license notice are retained.
