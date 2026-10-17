# Copilot Instructions for Seasonal Weather (Continued)

## Mod Overview and Purpose
The "Seasonal Weather (Continued)" mod is an extension and update to the original mod by AaronCRobinson. Its primary goal is to enhance the weather systems in RimWorld by introducing seasonal variations in weather patterns across different biomes. The mod aims to provide a more immersive and dynamic environmental experience, affecting gameplay and storytelling.

## Key Features and Systems
- **Seasonal Weather Changes**: Each season features unique weather patterns with commonalities across weather types.
- **Biome-Specific Weather Adaptations**: Weather is adjusted per biome to fit seasonal changes better.
- **New Weather Types**: Adds new weather phenomena including Dust, Dry Duststorm, Foggy Snow, Cloudy, and Windy.
- **Weather Disasters**: Introduces Wildfires and Earthquakes with configurable settings.
- **Dynamic Plant Growth**: During rainfall, plant growth rates are increased by 20%.
- **Mood Influences**: A new "Weather Drone" can affect colonist moods, integrating environmental impact on psychology.
- **Double Rainbows**: Rare events following rain that boost colonist mood.
- **Translations**: Available in English and Turkish.

## Coding Patterns and Conventions
- **C# Naming Conventions**: Classes and methods follow PascalCase, while variables use camelCase.
- **Separation of Concerns**: Game logic, such as weather events and systems, is separated into individual C# files for clarity and maintainability.
- **Harmony Patching**: Utilizes Harmony for non-destructive modification of game methods, ensuring compatibility with other mods.

## XML Integration
- XML files are used to define game conditions, incidents, and thought processes associated with new weather types.
- **Structure and Modularity**: XMLs are organized to separate different functionalities such as patches and weather adjustments, enabling easier updates and maintenance.
- **DefInjected Files**: Use these for injecting new definitions related to weather events and thought impacts.

## Harmony Patching
- The mod employs Harmony, a non-destructive patching framework, to safely modify existing game behavior.
- **Examples of Harmony Patches**:
  - `GameComponentUtility_FinalizeInit.cs`: Modifies game initialization to introduce seasonal weather setups.
  - `Plant_GrowthRate.cs`: Adjusts plant growth rates dynamically with weather events.
- **Patch Location**: All patches are grouped within the `HarmonyPatches` directory to maintain a clear organization.

## Suggestions for Copilot
To enhance development efficiency with GitHub Copilot:
- **Suggested Prompt**: Use comments and documentation to guide Copilot in generating consistent code that adheres to existing conventions.
- **Namespace Usage**: Ensure proper use of namespaces to avoid conflicts, especially when integrating Copilot-generated code.
- **Harmony Patch Templates**: Develop templates for common patching patterns that can be used by Copilot to assist in creating new patches.
- **XML File Management**: Use Copilot to automate the creation of boilerplate XML entries for new weather types and incidents.

By following these recommendations, contributors can leverage GitHub Copilot to further streamline the development process and maintain a coherent codebase for the "Seasonal Weather (Continued)" mod project.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

