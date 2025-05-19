<p align="center">
  <img src="./assets/mitoplayer.png" width="200" alt="MitoPlayer logo"/>
</p>

<h1 align="center">MitoPlayer 2024</h1>

<p align="center">
  A smart playlist assistant and custom MP3 player for DJs, focused on harmony and metadata-driven mixing.
</p>

# MitoPlayer 2024

**Custom MP3 player and playlist assistant for DJs**, designed to organize large music libraries, assign rich metadata, and generate harmonically compatible playlists — faster and more intuitively than traditional tools.

## 🎧 Why I built it

As a DJ, I needed a tool that helps me manage my music with custom properties like tone, BPM, vocal presence, and more. MitoPlayer lets me filter, organize, and preview tracks in a clean interface — with full control over playlist generation.

Originally built in **Java/Swing** without a database, the app was fully rewritten in 2024 using **C# (WinForms)** — first with **MySQL**, and later transitioned to **SQLite** to enable simpler deployment and zero external dependencies.

---

## ✨ Features

- 🎵 MP3 playback & playlist management  
- 🔑 BPM and key import (MixedInKey / VirtualDJ)  
- 🏷️ Custom metadata (tone, BPM, vocals, intro length, style, etc.)  
- 🧠 Advanced filtering & tag-based rules  
- 🖼️ Album cover browsing  
- 🎚️ VirtualDJ integration (live info display)  
- 📦 Track export & batch renaming  
- 🖱️ Drag-and-drop support with other DJ tools  
- 📋 Tag management with color coding  
- ⚙️ Rule system and template-based playlist generation *(coming soon)*  

---

## 📸 Screenshots

### Main Interface
![Main UI](./assets/main_ui.png)

### Filter & Tag View
![Filter View](./assets/filter_view.png)

### Tag Editor
![Tag Editor](./assets/tag_editor.png)

### Export Dialog
![Export](./assets/export_dialog.png)

---

## 🔧 Tech Stack

- C# (.NET 6)
- WinForms UI
- SQLite backend
- VirtualDJ / MixedInKey integration

---

## 🚧 Roadmap

- [ ] Playlist templates based on harmony rules  
- [ ] Full auto-mode playlist builder  
- [ ] Smart recommendations  
- [ ] Custom audio preview control  

---

## 📬 Contact

Feel free to reach out or open an issue if you're interested in using or contributing to the project.

---

## 🛡️ License

Proprietary – all rights reserved.
This software is distributed as proprietary and may not be copied, modified, or redistributed without explicit permission from the author.

However, the application makes use of several open-source components, which are included under their respective licenses. These third-party libraries are used in compliance with their license terms.

✅ Third-party components used:
Library	License	Source
NAudio (Core, Midi, Wasapi, WinForms, WinMM)	MIT	https://github.com/naudio/NAudio
Newtonsoft.Json	MIT	https://github.com/JamesNK/Newtonsoft.Json
SQLitePCLRaw (bundle_e_sqlite3, core, lib, provider)	MIT	https://github.com/ericsink/SQLitePCL.raw
System. (.NET runtime libraries)*	MIT	https://github.com/dotnet/runtime
TagLibSharp	LGPL-2.1-or-later	https://github.com/mono/taglib-sharp

All MIT-licensed components permit commercial use, modification, and redistribution, provided that the original license notice is retained.
The LGPL-licensed TagLibSharp is used in a manner compliant with dynamic linking rules.

📄 You can find a full list of licenses and attributions in the THIRD-PARTY-NOTICES.md file.
