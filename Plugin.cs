﻿using BepInEx;
using BepInEx.Logging;
using TerminalApi;
using TerminalApi.Classes;
using static TerminalApi.Events.Events;
using static TerminalApi.TerminalApi;

namespace TerminalShortcuts {
    [BepInPlugin(modGUID, modName, modVersion)]
    [BepInDependency("atomic.terminalapi")]
    [BepInDependency("verity.lethalos.api")]

    // https://thunderstore.io/c/lethal-company/p/Verity/LethalOS/v/1.0.7/
    // https://github.com/VerityIncorporated/LethalOS/issues/3
    public class Plugin : BaseUnityPlugin {

        private const string modGUID = "frog.terminalshortcuts";
        private const string modName = "Terminal Shortcuts Mod";
        private const string modVersion = "1.0.0";

        public static ManualLogSource LogSource { get; set; } = null!;

        private void Awake() {
            Logger.LogInfo("Plugin Test Plugin is loaded!");


            TerminalAwake += TerminalIsAwake;
            TerminalWaking += TerminalIsWaking;
            TerminalStarting += TerminalIsStarting;
            TerminalStarted += TerminalIsStarted;
            TerminalParsedSentence += TextSubmitted;
            TerminalBeginUsing += OnBeginUsing;
            TerminalBeganUsing += BeganUsing;
            TerminalExited += OnTerminalExit;
            TerminalTextChanged += OnTerminalTextChanged;
            // meow!  UwU >w<   


            // Will display 'Sorry but you cannot run kill' when 'run kill' is typed into the terminal
            // Will also display the same thing as above if you just type 'kill' into the terminal 
            // because the default verb will be 'run'
            AddCommand("kill", "Sorry but you cannot run kill\n", "run");

            // All the code below is essentially the same as the line of code above
            TerminalNode triggerNode = CreateTerminalNode($"Frank is not available right now.\n", true);
            TerminalKeyword verbKeyword = CreateTerminalKeyword("get", true);
            TerminalKeyword nounKeyword = CreateTerminalKeyword("frank");

            verbKeyword = verbKeyword.AddCompatibleNoun(nounKeyword, triggerNode);
            nounKeyword.defaultVerb = verbKeyword;

            AddTerminalKeyword(verbKeyword);

            // The second parameter passed in is a CommandInfo, if you want to have a callback.
            AddTerminalKeyword(nounKeyword, new() {
                TriggerNode = triggerNode,
                DisplayTextSupplier = () => {
                    return "This text will display";
                },
                Category = "Other",
                Description = "This is just a test command."
                // The above would look like '>FRANK\nThis is just a test command.' in Other
            });

            // Adds a new command/terminal keyword that is 'pop' and a callback function that will run when the node of the keyword is loaded
            AddCommand("pop", new CommandInfo() {
                DisplayTextSupplier = () => {
                    return "popped\n\n";
                },
                Category = "Other"
            });

            AddCommand("shortcuts", new CommandInfo() {
                DisplayTextSupplier = () => {
                    return """
                    Loot:
                    `$344` - There is $344 worth of loot left.
                    `7 ITM` - There are 7 items left in the facility

                    Time:
                    `3 PM` - It is 3 PM

                    -----------------------------

                    Monsters:
                    `BRAK` = Bracken
                    `JEST` = Jester
                    `GIAN` = Forest Giant
                    `COIL` = Coil Head
                    `SNAR` = Snare Flea
                    `MANR` = Maneater
                    `BTLR` = Butler

                    `BARB` = Barber (clay surgeon) 
                    `SPID` = Bunker Spider
                    `BUG` = Hoarding Bug
                    `SLIM` = Hygrodere (slime/blob)
                    `MASK` = Masked
                    `NUT` = Nutcracker
                    `SPOR` = Spore lizard
                    `THUM` = Thumper

                    `WORM` = Earth Leviathan
                    `HAWK` = Baboon Hawk (use if there is a large group that could pose a significant threat)
                    `DOG` = Eyeless Dog
                    `BIRD` = Old Bird


                    -----------------------------

                    Location Code:
                    `MAIN` = Main exit
                    `FIRE` = Fire exit
                    `HALL` = Other generic hallway
                    `SHIP` = Ship

                    -----------------------------

                    Player Codes:
                    SCIE = scienceboy
                    ASTN = aston
                    FROG = hugo
                    ALFI = alfie
                    MORG = morgan
                    LDAN = little dan
                    FDAN = fat dan
                    IRON = iron

                    -----------------------------

                    If someone has ghost girl we run the following:
                    `{PLAYERNAME} GIRL`

                    Death Protocol:
                    `{Player Code} DEAD`

                    Possession Protocol (Masked Conversion):
                    `{Player Code} MASK`

                    `UNK` or `?` = unidentified entity or other threat
                    """;
                },
                Category = "Other"
            });

            // Or

            AddCommand("push", new CommandInfo() {
                DisplayTextSupplier = CommandFunction,
                Category = "Misc" // Does not work for categories that do not exist, yet ;)
            });
        }

        private string CommandFunction() {
            return "Wait, you cannot push\n\n";
        }

        private void OnTerminalTextChanged(object sender, TerminalTextChangedEventArgs e) {
            string userInput = GetTerminalInput();
            Logger.LogMessage(userInput);
            // Or
            Logger.LogMessage(e.CurrentInputText);

            // If user types in fuck it will changed to frick before they can even submit
            if (userInput == "fuck") {
                SetTerminalInput("frick");
            }

            if (userInput == "tm")
            {
                SetTerminalInput("transmit ");
            }

        }

        private void OnTerminalExit(object sender, TerminalEventArgs e) {
            Logger.LogMessage("Terminal Exited");
        }

        private void TerminalIsAwake(object sender, TerminalEventArgs e) {
            Logger.LogMessage("Terminal is awake");

            ////Adds 'Hello' as a new line to the help node
            //NodeAppendLine("help", "\nHello");
        }

        private void TerminalIsWaking(object sender, TerminalEventArgs e) {
            Logger.LogMessage("Terminal is waking");
        }

        private void TerminalIsStarting(object sender, TerminalEventArgs e) {
            Logger.LogMessage("Terminal is starting");
        }

        private void TerminalIsStarted(object sender, TerminalEventArgs e) {
            Logger.LogMessage("Terminal is started");
        }

        private void TextSubmitted(object sender, TerminalParseSentenceEventArgs e) {
            Logger.LogMessage($"Text submitted: {e.SubmittedText} Node Returned: {e.ReturnedNode}");
        }

        private void OnBeginUsing(object sender, TerminalEventArgs e) {
            Logger.LogMessage("Player has just started using the terminal");
        }

        private void BeganUsing(object sender, TerminalEventArgs e) {
            Logger.LogMessage("Player is using terminal");
        }

    }

}



