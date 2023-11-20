using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerController : MonoBehaviour
{
    public string villagerName;
    public KeyCode interactionKey = KeyCode.E;
    public DialogManager dialogManager;
    public SceneInfo playerStorage;

    private Queue<string> dialogLines = new Queue<string>();
    private PlayerController playerController;
    private VillagerLifeController villagerLifeController;
    private bool isPlayerInRange = false;
    private bool isConversationDry = true;
    private Dictionary<string, int> dialogIndices = new Dictionary<string, int>()
    {
        {"Police", 0},
        {"Mayor", 1},
        {"Samuel", 2},
        {"Isabel", 3},
        {"Lillian", 4},
        {"Walter", 5},
    };


    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        villagerLifeController = GetComponent<VillagerLifeController>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Check if the colliding object is the player
        if (collision.gameObject.tag == "Player")
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the colliding object is the player
        if (collision.gameObject.tag == "Player")
        {
            isPlayerInRange = false;
        }
    }

    private void Update()
    {
        // If queue is empty, add dialog
        if (dialogLines.Count == 0)
        {
            AddDialog();
        }
        if (isPlayerInRange && Input.GetKeyDown(interactionKey))
        {
            if (dialogLines.Count > 0)
            {
                isConversationDry = false;
            }
            this.NextLine();
        }
    }

    private void NextLine()
    {
        if (dialogManager.IsActive() && !dialogManager.HasReachedEnd())
        {
            dialogManager.ContinueDialog();
            return;
        }
        // If dialogLines is empty, hide the dialog box)
        if (dialogLines.Count == 0)
        {
            if (isConversationDry)
            {
                // Set the dialogue to a random dry dialogue
                dialogManager.ShowDialog();
                SetDialogAndLabel(DryDialogue());
                playerController.isInDialog = true;
                villagerLifeController.isInDialog = true;
                isConversationDry = false;
                return;
            }
            dialogManager.HideDialog();
            playerController.isInDialog = false;
            villagerLifeController.isInDialog = false;
            isConversationDry = true;
            return;
        }
        string dialog = dialogLines.Dequeue();
        if (dialog.StartsWith("System: "))
        {
            SystemPrompt(dialog);
            NextLine();
            return;
        }
        dialogManager.ShowDialog();
        playerController.isInDialog = true;
        villagerLifeController.isInDialog = true;
        SetDialogAndLabel(dialog);
    }

    private void SystemPrompt(string prompt)
    {
        // If prompt starts with "System: ", then it's a system prompt
        // System prompts are everything after "System: " and it indicates which playerStorage.dialogueRead[] to update
        string systemPrompt = prompt.Substring(8);
        switch(systemPrompt)
        {
            case "base":
                playerStorage.dialogueRead[dialogIndices[villagerName]].baseDialogue = true;
                break;
            case "clueA":
                playerStorage.dialogueRead[dialogIndices[villagerName]].clueAFound = true;
                break;
            case "clueB":
                playerStorage.dialogueRead[dialogIndices[villagerName]].clueBFound = true;
                break;
            case "clueC":
                playerStorage.dialogueRead[dialogIndices[villagerName]].clueCFound = true;
                break;
        }
    }


    private string DryDialogue()
    {
        int location = -1;
        switch (villagerName)
        {
            case "Police":
                location = villagerLifeController.GetCurrentLocation();
                Debug.Log("Villager's location is " + location);
                switch (location)
                {
                    case 0:
                        return "Villager: I sometimes come out to this lake to think.";
                    case 1:
                        return "Villager: I'm just looking for what we missed here...";
                    case 2: // Furniture
                        return "Villager: Don't forget to turn in your evidence in the evidence box when you cracked the case!";
                    case 3: // Desk
                        return "Villager: I'm working on it detective, I'm working on it...";
                    default:
                        break;
                }
                return "Villager: I don't have anything to say to you.";
            case "Mayor":
            case "Samuel":
            case "Isabel":
            case "Lillian":
            case "Walter":
            default:
                return "Villager: I don't have anything to say to you.";
        }
        return "Villager: I don't have anything to say to you.";
    }

    // Determines which dialog to read based on...
    // what dialogue has already been read
    // what clues have already been found
    private void AddDialog()
    {
        switch (villagerName)
        {
            case "Police":
                if (!playerStorage.dialogueRead[0].baseDialogue)
                {
                    dialogLines.Enqueue("Villager: Detective, thank goodness you're here! We've called upon you because you're one of the best investigators around Willowbrook. Quite frankly, you're our last hope in solving this crime.");
                    dialogLines.Enqueue("Detective: What's happened here officer?");
                    dialogLines.Enqueue("Villager: Early this morning, Evelyn Greenfield, the village florist, was found dead in the forest. It's a small village and we've never had a case like this before!");
                    dialogLines.Enqueue("Detective: I see. How much do we already know officer?");
                    dialogLines.Enqueue("Villager: Not much I'm afraid. Explore the village, gather clues, and talk to the villagers. We need 8 clues total before we can prosecute");
                    dialogLines.Enqueue("Villager: You should have a journal, with all your clues, that you can see by pressing J. Go to my office to collect your first clue. It's a detailed book I've prepared for you with all the information we have so far.");
                    dialogLines.Enqueue("Detective: Thank you officer. I'll get to work right away.");
                    dialogLines.Enqueue("Villager: Good luck, Detective. Willowbrook is counting on you to bring justice for Evelyn!");
                    dialogLines.Enqueue("System: base");
                }
                else if (playerStorage.cluesFound[6] && !playerStorage.dialogueRead[0].clueAFound) // Found blueprint
                {
                    dialogLines.Enqueue("Detective: Chief Harper, I found a blueprint outside Mayor Whitaker's house. It suggests he had plans to relocate Evelyn's florist shop. Did you know about this?");
                    dialogLines.Enqueue("Villager: A blueprint, you say? No, Detective, I wasn't aware of any such plans. Mayor Whitaker never mentioned anything about relocating Evelyn's shop to me.");
                    dialogLines.Enqueue("Detective: It seems the mayor wanted to expand the village square and believed relocating the florist shop would benefit Willowbrook. Do you think this information is relevant to Evelyn's murder?");
                    dialogLines.Enqueue("Villager: Well, it certainly adds another layer to the mystery. If the mayor's intentions were genuine, then he might not have a motive to harm Evelyn directly. But we can't rule out the possibility that someone misunderstood his plans or had a different agenda altogether.");
                    dialogLines.Enqueue("Detective: Mayor Whitaker claims he submitted the proposal to the village council. I'll need to verify this with them. In the meantime, Chief, is there anything else you can tell me about the mayor's relationship with Evelyn? Any previous incidents or disputes?");
                    dialogLines.Enqueue("Villager: I can't say there were any major incidents, Detective. Mayor Whitaker and Evelyn did have their differences in terms of the village's direction, but nothing that escalated to violence or threats. They were both passionate about their beliefs, but as far as I know, it didn't go beyond that.");
                    dialogLines.Enqueue("Detective: I'll keep investigating, Chief. This blueprint might be a dead end, or it could be a crucial piece of the puzzle. I'll update you once I learn more.");
                    dialogLines.Enqueue("Villager: Alright, Detective. Keep me posted, and don't hesitate to reach out if you need any assistance. We're counting on you to unravel this mystery and bring justice to Evelyn's memory.");
                    dialogLines.Enqueue("System: clueA");
                }
                break;
            case "Mayor":
                if (!playerStorage.dialogueRead[1].baseDialogue)
                {
                    dialogLines.Enqueue("Detective: Mayor Whitaker, did you have any issues with Evelyn?");
                    dialogLines.Enqueue("Villager: Well, Detective, I won't deny our differences. I believe progress is necessary for our village, but Evelyn clung to tradition. Still, I would never resort to violence.");
                    dialogLines.Enqueue("Detective: Mayor Whitaker, some villagers say your push for modernization put you in conflict with Evelyn. Can you elaborate on that?");
                    dialogLines.Enqueue("Villager: Ah, Detective, you've heard the rumors, I see. Yes, it's true that Evelyn and I had our disagreements. I believe Willowbrook needs to evolve with the times, embrace technology, and attract more visitors. But Evelyn was adamant about preserving the village's traditions, and she saw me as a threat to that.");
                    dialogLines.Enqueue("Detective: Were these disagreements serious enough to lead to violence?");
                    dialogLines.Enqueue("Villager: Absolutely not! While our debates could get heated, I would never resort to violence. I always believed that our differences added flavor to our village. Besides, I had nothing to gain from Evelyn's death. In fact, it's quite the opposite. This incident has cast a shadow over my plans for Willowbrook's future.");
                    dialogLines.Enqueue("Detective: Is there anyone who might have wanted to harm Evelyn to further their own agenda?");
                    dialogLines.Enqueue("Villager: Well, I can't say for sure, Detective. But if you're looking for motives, there are others who were more directly affected by Evelyn's actions. Perhaps you should talk to them. I'm committed to working with you to uncover the truth behind this dreadful event.");
                    dialogLines.Enqueue("System: base");
                }
                else if (playerStorage.cluesFound[6] && !playerStorage.dialogueRead[1].clueAFound) // Found blueprint
                {
                    dialogLines.Enqueue("Detective: Mayor Whitaker, I found this blueprint outside your house that indicates your plan to take over Evelyn's florist shop. Care to explain?");
                    dialogLines.Enqueue("Villager: Ah, Detective, I see you've stumbled upon that old thing. Yes, it's true, I did commission a blueprint for the florist shop, but it's not what it seems.");
                    dialogLines.Enqueue("Detective: Not what it seems? The blueprint clearly shows your intentions to tear down Evelyn's shop.");
                    dialogLines.Enqueue("Villager: Detective, please allow me to clarify. The blueprint is part of a proposal I submitted to the village council. I wanted to expand the village square, create a more welcoming space for everyone. Evelyn's shop would be relocated, not destroyed.");
                    dialogLines.Enqueue("Detective: Relocated? Why didn't you discuss this with Evelyn directly?");
                    dialogLines.Enqueue("Villager: I tried, Detective. I really did. But Evelyn was staunchly against any changes to her shop or the village layout. She refused to even consider the idea, and our discussions always ended in disagreement. I thought presenting the proposal to the council would be a more diplomatic approach.");
                    dialogLines.Enqueue("Detective: So, you're saying your plan was to improve the village, not harm Evelyn?");
                    dialogLines.Enqueue("Villager: Exactly, Detective. My vision for Willowbrook includes progress and growth, but not at the cost of harming anyone. I regret that Evelyn had to see it this way. If anything, her death has disrupted the harmony I hoped to achieve for our community.");
                    dialogLines.Enqueue("Detective: Did Evelyn know about this proposal before her death?");
                    dialogLines.Enqueue("Villager: I'm not sure, Detective. She was quite resistant to change, and I feared bringing it up would only cause more tension. Now, with her gone, it seems my intentions have been misunderstood.");
                    dialogLines.Enqueue("Detective: I'll need to verify this with other sources. If you're telling the truth, someone else might have seen this proposal. In the meantime, I'll continue my investigation. Thank you for your explanation, Mayor Whitaker.");
                    dialogLines.Enqueue("System: clueA");
                }
                else if (playerStorage.cluesFound[2] && !playerStorage.dialogueRead[1].clueBFound) // Found knife
                {
                    dialogLines.Enqueue("Detective: Mayor Whitaker, I have some questions about this red-stained knife. People in the village have mentioned that you were in possession of it. Care to explain?");
                    dialogLines.Enqueue("Villager: Ah, Detective, I see you've been busy gathering information. Yes, I did borrow that knife from Isabel a couple of weeks ago. Needed it for a small DIY project at the town hall. You know how it is, always something that needs fixing.");
                    dialogLines.Enqueue("Detective: You borrowed the knife for a project at the town hall? Can you provide more details about what exactly you were working on?");
                    dialogLines.Enqueue("Villager: Just some repairs and renovations, Detective. Nothing too exciting. I had to cut some materials, and that knife seemed handy at the time. Why do you ask?");
                    dialogLines.Enqueue("Detective: There's a red stain on the blade, Mayor. Any idea what that might be from? It's important to the investigation.");
                    dialogLines.Enqueue("Villager: Red stain, you say? Odd. I don't recall anything particularly messy about the project. Maybe it was some paint or rust on the knife already. You know how tools can get. But I assure you, Detective, it has nothing to do with Evelyn's unfortunate situation.");
                    dialogLines.Enqueue("Detective: Mayor, do you often borrow tools from other villagers for your projects?");
                    dialogLines.Enqueue("Villager: Absolutely, Detective. We're a tight community here in Willowbrook. We help each other out whenever we can. Isabel and I exchange tools quite often. It keeps things running smoothly. But I can assure you, my use of that knife had nothing to do with what happened to Evelyn.");
                    dialogLines.Enqueue("Detective: People in the village are concerned about your plans to tear down Evelyn's florist shop, as seen in this blueprint. Any comments on that, Mayor?");
                    dialogLines.Enqueue("Villager: Ah, the blueprint. Yes, I've been advocating for some modernization in Willowbrook. The florist shop is a bit outdated, you see. But tearing it down? No, no. That's just a proposal for potential renovations. We can't forget our traditions, Detective. I wouldn't do anything to harm the village or its beloved establishments.");
                    dialogLines.Enqueue("Detective: I appreciate your cooperation, Mayor. If you remember anything else about the knife or any other relevant information, please let me know.");
                    dialogLines.Enqueue("Villager: Certainly, Detective. I'm here to assist in any way I can. Let's hope you get to the bottom of this mystery soon.");
                    dialogLines.Enqueue("System: clueB");
                }
                break;
            case "Samuel":
                if (!playerStorage.dialogueRead[2].baseDialogue)
                {
                    dialogLines.Enqueue("Detective: Samuel, did you and Evelyn have a close relationship?");
                    dialogLines.Enqueue("Villager: Close? Yes, in a way. I admired her work, but she never saw me as more than a friend. It's heartbreaking that she's gone.");
                    dialogLines.Enqueue("Detective: Samuel, you and Evelyn seem to have been close. Can you tell me about your relationship with her?");
                    dialogLines.Enqueue("Villager: Ah, Evelyn. Yes, we were close in a way. She loved my stories and often came by to chat about them. I admired her greatly, Detective. She had a passion for her flowers that I found enchanting.");
                    dialogLines.Enqueue("Detective: It sounds like you had feelings for her. Did your closeness ever lead to conflicts?");
                    dialogLines.Enqueue("Villager: Yes, Detective, you've caught me. I was in love with Evelyn, and I thought she felt the same way. But she never saw me as more than a friend. It was a source of heartache for me, but I would never harm her. I cherished our friendship.");
                    dialogLines.Enqueue("Detective: Were you aware of any enemies or disputes Evelyn had that might have led to her murder?");
                    dialogLines.Enqueue("Villager: I knew she had her disagreements, particularly with the mayor and Isabel. But to think that someone would kill her over it? It's unthinkable. Evelyn was a kind soul, and Willowbrook won't be the same without her.");
                    dialogLines.Enqueue("Detective: Did Evelyn ever share any secrets or concerns with you before her death?");
                    dialogLines.Enqueue("Villager: I wish she had, Detective. The last time we spoke, she seemed distant, as if she were carrying a heavy burden. But she didn't confide in me about the specifics. Maybe it's connected to her murder. I wish I could help more.");
                    dialogLines.Enqueue("System: base");
                }
                else if (playerStorage.cluesFound[1] && !playerStorage.dialogueRead[2].clueAFound) // Found letter
                {
                    dialogLines.Enqueue("Detective: Samuel, I came across a letter in Evelyn's belongings. It seems to be from you.");
                    dialogLines.Enqueue("Villager: Ah, that letter. I never intended for anyone to read it. It's embarrassing, really.");
                    dialogLines.Enqueue("Detective: The letter suggests you had deep feelings for Evelyn. Can you elaborate on your relationship with her?");
                    dialogLines.Enqueue("Villager: Detective, yes, it's true. I had strong feelings for Evelyn. We were close, and I couldn't help but fall for her. But she never reciprocated those feelings. It was a painful truth I had to live with.");
                    dialogLines.Enqueue("Detective: In the letter, you mention a desire to protect her. Did you feel the need to protect Evelyn from something specific?");
                    dialogLines.Enqueue("Villager: Well, yes. I wanted to shield her from any harm or troubles that might come her way. Evelyn had a way of getting entangled in disputes, especially with the mayor and Isabel. I wanted to be there for her, even if it meant silently watching from the shadows.");
                    dialogLines.Enqueue("Detective: Given your closeness, did Evelyn ever confide in you about any concerns or secrets before her death?");
                    dialogLines.Enqueue("Villager: I wish she had, but she didn't. The last time we spoke, she seemed burdened, distant. I sensed something was amiss, but she didn't share the details. If only I could have been of more help to her.");
                    dialogLines.Enqueue("Detective: Did your feelings for Evelyn ever lead to conflicts or disagreements between you two?");
                    dialogLines.Enqueue("Villager: Yes Detective. Once when I was walking around the woods, I saw many men walk into the florist shop around 9:00 PM. I asked Evelyn what it was about and she thought I was lying.");
                    dialogLines.Enqueue("Detective: Men entering the florist shop? Can you describe them or recall any specific details?");
                    dialogLines.Enqueue("Villager: It was dark, and they were mostly silhouettes. I couldn't make out their faces, but there were several of them and they were quite bulky. It struck me as odd because the shop should have been closed at that time. At the time, I thought they were security guards. Evelyn dismissed it, saying it was probably my imagination. But I couldn't shake the feeling that something was off.");
                    dialogLines.Enqueue("Detective: Did Evelyn mention anything about these men afterward? Any unusual occurrences or changes in her behavior?");
                    dialogLines.Enqueue("Villager: No, she didn't mention it again but her behavior considerably changed in the following weeks. I wish I had insisted more, but I was too in love to ask her such questions. Now, I can't help but wonder if it had something to do with her murder.");
                    dialogLines.Enqueue("Detective: Thank you, Samuel. Your information is valuable. If you remember anything else, please don't hesitate to let me know.");
                    dialogLines.Enqueue("System: clueA");
                }
                break;
            case "Isabel":
                if (!playerStorage.dialogueRead[3].baseDialogue)
                {
                    dialogLines.Enqueue("Detective: Isabel, were you in any conflicts with Evelyn before her death?");
                    dialogLines.Enqueue("Villager: Oh, Detective, you've caught me. We had our spats, especially during the fair. But I wouldn't kill her over that. It was just rivalry.");
                    dialogLines.Enqueue("Detective: Isabel, I heard there was some rivalry between you and Evelyn. Can you tell me more about it?");
                    dialogLines.Enqueue("Villager: Detective, rivalry might be an understatement. Evelyn and I have been competing in the village fair for years, especially in the baking contest. She always had a way of getting under my skin.");
                    dialogLines.Enqueue("Detective: Did this rivalry ever escalate into something more serious?");
                    dialogLines.Enqueue("Villager: Well, we've had our fair share of arguments, especially when it came to who makes the best pies. But I would never hurt anyone over a baking contest. It was all in good fun, you know?");
                    dialogLines.Enqueue("Detective: Were there any recent conflicts or events between you and Evelyn that might have escalated things?");
                    dialogLines.Enqueue("Villager: Now that you mention it, Detective, the last fair was particularly heated. Evelyn accused me of using some secret ingredient, which I denied. But I thought we moved past it.");
                    dialogLines.Enqueue("Detective: Did Evelyn ever mention any other disputes or grudges she had with anyone else in the village?");
                    dialogLines.Enqueue("Villager: I can't say for sure, but Evelyn was pretty vocal about her opinions. She clashed with the mayor and Walter over her flower shop. And she had disagreements with Lillian too. But none of it seemed serious enough to lead to something like this.");
                    dialogLines.Enqueue("System: base");
                }
                else if (playerStorage.cluesFound[2] && !playerStorage.dialogueRead[3].clueAFound) // Found knife
                {
                    dialogLines.Enqueue("Detective: Isabel, I found this red-stained knife outside your house. Care to explain?");
                    dialogLines.Enqueue("Villager: Oh, Detective, you found that, huh? Well, I can explain. I lent that knife to Mayor Whitaker about two weeks ago. Can't quite recall why he needed it, though.");
                    dialogLines.Enqueue("Detective: You lent this knife to Mayor Whitaker? Do you remember anything specific about why he borrowed it?");
                    dialogLines.Enqueue("Villager: Honestly, Detective, it slipped my mind. The mayor and I don't exactly keep a detailed record of every little thing. He just needed it for something, and I thought, why not?");
                    dialogLines.Enqueue("Detective: You can't recall any details about what the mayor wanted the knife for?");
                    dialogLines.Enqueue("Villager: No, Detective, I can't. We exchange tools and things from time to time. It's a small village, you know. People help each other out. But if that knife has something to do with Evelyn's... situation, well, that's news to me.");
                    dialogLines.Enqueue("Detective: Isabel, do you often lend your tools to other villagers?");
                    dialogLines.Enqueue("Villager: Sure, Detective, we all do. Living in a close-knit community like Willowbrook, it's only natural to help each other out. The mayor and I have exchanged tools before. It's nothing out of the ordinary.");
                    dialogLines.Enqueue("Detective: The red stain on this knife, any idea what it could be? It might be important to the case.");
                    dialogLines.Enqueue("Villager: Red stain? Huh, can't say I noticed that before. Maybe the mayor used it for something messy. You know how he is, always getting his hands into this and that. But I swear, Detective, I didn't harm Evelyn with that knife.");
                    dialogLines.Enqueue("Detective: Alright, Isabel. I'll need to investigate this further. If you remember anything else about the knife or the mayor's use of it, let me know.");
                    dialogLines.Enqueue("Villager: Of course, Detective. I'm as curious as you are about all this. I hope you find the truth.");
                    dialogLines.Enqueue("System: clueA");
                }
                else if (playerStorage.cluesFound[4] && !playerStorage.dialogueRead[3].clueBFound) // Found note from diary
                {
                    dialogLines.Enqueue("Detective: Isabel, I found this torn note in Evelyn's diary. It mentions a confrontation she had, and she seems hurt. Can you shed some light on this?");
                    dialogLines.Enqueue("Villager: Oh, Detective, a note... I didn't think anyone would find out about that. Yeah, we had an argument before she died. It's been weighing on me, and I regret it so much.");
                    dialogLines.Enqueue("Detective: Can you tell me more about this argument? What was it about, and why did it happen?");
                    dialogLines.Enqueue("Villager: It was stupid, really. We were arguing about the village fair, the usual stuff. But this time, it got heated. Evelyn accused me of something, something secret ingredient-related, and I got defensive. I didn't think it would lead to... well, you know.");
                    dialogLines.Enqueue("Detective: And what exactly did she accuse you of? Can you recall any details of the argument?");
                    dialogLines.Enqueue("Villager: She thought I was using some secret ingredient in my pies, something to give me an edge in the baking contest. But I swear, Detective, I would never do something like that. It's just a fair competition, you know?");
                    dialogLines.Enqueue("Detective: Did the argument escalate? Were there any other people involved?");
                    dialogLines.Enqueue("Villager: No, it was just me and Evelyn. We were both upset, and I stormed off. I wish I hadn't let it get to that point. But, Detective, I need you to believe me; I would never harm Evelyn over something so trivial.");
                    dialogLines.Enqueue("Detective: Is there anything else you can tell me about this argument? Any details that might help in the investigation?");
                    dialogLines.Enqueue("Villager: Well, she was upset, and I could tell it hurt her too. But we didn't talk after that. I didn't see her again until Chief Harper told me she was gone. I've been carrying the guilt of our last words ever since.");
                    dialogLines.Enqueue("Detective: Thank you for sharing, Isabel. I appreciate your honesty. Now, were there any other conflicts or arguments Evelyn had with someone else in the village that you're aware of?");
                    dialogLines.Enqueue("Villager: Conflicts with others? Well, she clashed with the mayor and Walter over her flower shop. And she had disagreements with Lillian too. But none of it seemed serious enough to lead to something like this.");
                    dialogLines.Enqueue("Detective: Alright, thank you for your cooperation, Isabel. If you remember anything else or if you hear anything from the other villagers, please let me know.");
                    dialogLines.Enqueue("System: clueB");
                }
                break;
            case "Lillian":
                if (!playerStorage.dialogueRead[4].baseDialogue)
                {
                    dialogLines.Enqueue("Detective: Lillian, can you tell me about your relationship with Evelyn?");
                    dialogLines.Enqueue("Villager: Evelyn was like a mother to me, Detective. We had our disagreements, sure, but I would never harm her. She was about to reveal something, though. Something big.");
                    dialogLines.Enqueue("Detective: Lillian, I've heard you had a close relationship with Evelyn. Can you tell me more about it?");
                    dialogLines.Enqueue("Villager: Yes, Detective. Evelyn took me under her wing when I first arrived in Willowbrook. She taught me everything I know about flowers and running a shop. We had our differences, but I owe her everything.");
                    dialogLines.Enqueue("Detective: Differences? What kind of disagreements did you have?");
                    dialogLines.Enqueue("Villager: Evelyn was traditional, and I wanted to try new things. Sometimes, we clashed over how to arrange flowers or the direction the shop should take. But it was all in the spirit of making our shop better.");
                    dialogLines.Enqueue("Detective: Did Evelyn confide in you about anything unusual or concerning before her death?");
                    dialogLines.Enqueue("Villager: Actually, yes. Just before she died, she said she was about to uncover something that could change everything. She looked worried, Detective, more worried than I've ever seen her.");
                    dialogLines.Enqueue("Detective: Did she mention who or what this 'something' was?");
                    dialogLines.Enqueue("Villager: No, she didn't. She said she needed more time to gather evidence. I wish I knew what she was talking about. It's been eating at me ever since.");
                    dialogLines.Enqueue("System: base");
                }
                else if (playerStorage.cluesFound[4] && !playerStorage.dialogueRead[4].clueAFound) // Found note from diary
                {
                    dialogLines.Enqueue("Detective: Lillian, I found this loose page in Evelyn's shop. It seems to be from her diary. Care to explain what she meant by this?");
                    dialogLines.Enqueue("Villager: Oh, that? I don't know anything about it, Detective. I've never seen that page before.");
                    dialogLines.Enqueue("Detective: Lillian, this page mentions a confrontation with someone. Evelyn seemed upset. Do you have any idea what she was talking about?");
                    dialogLines.Enqueue("Villager: Alright, Detective. Evelyn and I had our differences, as I mentioned before. She wanted to stick to tradition, and I wanted to try new things. One day, I confronted her about it. She swore she had no idea what I was talking about.");
                    dialogLines.Enqueue("Detective: Confronted her about what exactly?");
                    dialogLines.Enqueue("Villager: About the way she was running the shop. I thought she was holding us back, stuck in her old-fashioned ways. It's a shame she never had the courage to change.");
                    dialogLines.Enqueue("Detective: It seems she was worried about something else, something more serious. Did she ever confide in you about any concerns she had?");
                    dialogLines.Enqueue("Villager: No, she didn't. I mean, she did mention she was about to uncover something big, but she never told me what it was. I wish I could help you more, Detective.");
                    dialogLines.Enqueue("Detective: This loose page suggests that not everyone had the courage to be honest with Evelyn. Do you think there's someone in the village who might have known more about what she was investigating?");
                    dialogLines.Enqueue("Villager: I... I don't know, Detective. Evelyn kept to herself a lot. Maybe there's someone else in the village who knew more about her affairs. I just wish she had trusted me enough to share whatever it was she was dealing with.");
                    dialogLines.Enqueue("System: clueA");
                }
                else if (playerStorage.cluesFound[3] && !playerStorage.dialogueRead[4].clueBFound) // Found poisonous flower
                {
                    dialogLines.Enqueue("Detective: Lillian, I found this unusual flower near the woods. It seems to glow at night. Do you know anything about it?");
                    dialogLines.Enqueue("Lillian: Oh, that? It's just a rare species I stumbled upon while collecting flowers for the shop. Walter, the retired botanist, would know more about it. He's an expert in all things botanical.");
                    dialogLines.Enqueue("Detective: Walter, huh? Did Evelyn ever mention this particular flower or ask you to gather it?");
                    dialogLines.Enqueue("Lillian: No, Detective. Evelyn was more focused on her traditional flowers. She didn't pay much attention to the rare ones I brought in. If anything, she and Walter had their fair share of disagreements about introducing new species to the village.");
                    dialogLines.Enqueue("Detective: Disagreements with Walter, you say? What were those about?");
                    dialogLines.Enqueue("Lillian: Walter is very protective of the village's flora. He believes in preserving the natural balance and was against introducing exotic flowers. Evelyn, on the other hand, thought diversifying the shop's offerings could attract more customers. They argued about it more than once.");
                    dialogLines.Enqueue("Detective: So, Walter and Evelyn had their disagreements about these flowers. Did it ever escalate?");
                    dialogLines.Enqueue("Lillian: Well, they had heated debates, but nothing too serious. Evelyn respected Walter's knowledge, even if she didn't always agree with him. I doubt he'd harm her over a disagreement about flowers.");
                    dialogLines.Enqueue("Detective: Interesting. Thanks for the information, Lillian. I might have a chat with Walter to see if he knows anything more about this flower.");
                    dialogLines.Enqueue("System: clueB");
                }
                else if (playerStorage.cluesFound[5] && !playerStorage.dialogueRead[4].clueCFound) // Found potion
                {
                    dialogLines.Enqueue("Detective: Lillian, I found this potion in the underground lair beneath the florist shop. Care to explain why you had it hidden there?");
                    dialogLines.Enqueue("Villager: Detective, I have no idea what you're talking about. An underground lair? That's preposterous! And a potion? I can't even fathom why something like that would be in Evelyn's shop.");
                    dialogLines.Enqueue("Detective: Lillian, I've been piecing together clues, and it seems like you were involved in making this deadly potion. Can you really look me in the eye and deny it?");
                    dialogLines.Enqueue("Villager: A deadly potion? This is absurd! I would never harm Evelyn. I loved her like a mother, as I've said before. And an underground lair? I had no idea it even existed.");
                    dialogLines.Enqueue("Detective: I find it hard to believe, Lillian. The evidence points to you, and now there's this hidden space beneath the florist shop. Why would you keep such a thing secret if you had nothing to hide?");
                    dialogLines.Enqueue("Villager: Detective, I swear, I had no knowledge of any underground lair or deadly potion. I can't explain how it got there, but I can tell you it has nothing to do with me. Perhaps Walter, the retired botanist, had something to do with this. He used to work in the florist house, you know. Maybe he's the one behind all of this.");
                    dialogLines.Enqueue("Detective: Walter? Are you sure about that, Lillian? He doesn't strike me as someone who would concoct a deadly potion.");
                    dialogLines.Enqueue("Villager: You never know, Detective. People can surprise you. Maybe he held a grudge against Evelyn for some reason. You should look into him and his motives. I'm innocent, I promise.");
                    dialogLines.Enqueue("Detective: I'll keep that in mind, Lillian. But if you're innocent, why were you so hesitant to admit you knew about the underground lair? It seems like you're hiding something.");
                    dialogLines.Enqueue("Villager: I swear, Detective, I had no idea. I'm just as shocked as you are. Please find the real culprit and clear my name. I loved Evelyn, and I want justice for her as much as anyone.");
                    dialogLines.Enqueue("System: clueC");
                }
                break;

            case "Walter":
                if (!playerStorage.dialogueRead[5].baseDialogue)
                {
                    dialogLines.Enqueue("Detective: Walter, did you and Evelyn have professional disagreements?");
                    dialogLines.Enqueue("Villager: Indeed, we did. I'm a scientist, and she was all about those newfangled methods. But violence? That's absurd.");
                    dialogLines.Enqueue("Detective: Walter, I've heard there were disagreements between you and Evelyn. Can you elaborate on that?");
                    dialogLines.Enqueue("Villager: Ah, Evelyn and I had our differences, Detective. She was all about her modern flower techniques, and I'm a staunch traditionalist when it comes to botany. We often debated the future of the shop and the village.");
                    dialogLines.Enqueue("Detective: Did these disagreements ever become particularly heated?");
                    dialogLines.Enqueue("Villager: Heated, yes, but never violent. We were both passionate about our views, but I would never resort to harm over a difference in opinion. That's simply not my way.");
                    dialogLines.Enqueue("Detective: Were there any recent disputes or incidents between you and Evelyn that might have escalated things?");
                    dialogLines.Enqueue("Villager: Now that you mention it, Detective, just before her death, Evelyn had been experimenting with some unusual flowers. She claimed they were a game-changer, but I thought they were dangerous. We had a heated argument about it.");
                    dialogLines.Enqueue("Detective: Did Evelyn confide in you about any other conflicts or secrets before her death?");
                    dialogLines.Enqueue("Villager: I can't say she did. She was rather secretive about her personal affairs. It's unfortunate that it's come to this, but I assure you, I had nothing to do with her death.");
                    dialogLines.Enqueue("System: base");

                }
                else if (playerStorage.cluesFound[4] && !playerStorage.dialogueRead[1].clueAFound) // Found Note from diary
                {
                    dialogLines.Enqueue("Detective: Walter, I found this torn page from Evelyn's diary. She mentions confronting someone and feeling betrayed. Do you have any idea who these people she's talking about might be?");
                    dialogLines.Enqueue("Villager: Confrontations and betrayal? That's rather perplexing. In a small village like Willowbrook, everyone knows everyone else's business. I can't imagine who she might be referring to. Perhaps they're from out of town? Evelyn did have a lot of customers who came from afar to buy her unique flowers.");
                    dialogLines.Enqueue("Detective: Did you have any loyal customers when you used to work in the florist shop?");
                    dialogLines.Enqueue("Villager: Oh, yes, I did have my fair share of loyal customers, but they were more interested in traditional botanical specimens rather than Evelyn's modern creations. I can't fathom who these mysterious individuals might be. Willowbrook is a tight-knit community, and it's hard to keep secrets here.");
                    dialogLines.Enqueue("Detective: Considering the content of this note, do you think these people might be connected to her death?");
                    dialogLines.Enqueue("Villager: I find it hard to believe that anyone in Willowbrook would resort to violence. It's a peaceful village, and though we may have our differences, harming someone is beyond the scope of our disputes. If there is a connection, it's a mystery to me.");
                    dialogLines.Enqueue("Detective: Did Evelyn ever mention any unusual occurrences or suspicious activities in or around the florist shop?");
                    dialogLines.Enqueue("Villager: No, Detective, she never mentioned anything like that to me. As far as I knew, her focus was on her flowers and the occasional disagreement with me over botanical matters. If there were secrets, she kept them well-hidden.");
                    dialogLines.Enqueue("Detective: I appreciate your honesty, Walter. If you think of anything else that might help, please let me know.");
                    dialogLines.Enqueue("Villager: Of course, Detective. I want to see justice served for Evelyn's sake and for the sake of our peaceful village.");
                    dialogLines.Enqueue("System: clueA");

                }
                else if (playerStorage.cluesFound[3] && !playerStorage.dialogueRead[1].clueBFound) // Found poisonous flower
                {
                    dialogLines.Enqueue("Detective: Walter, I found this mysterious flower in the woods. Can you take a look at it and tell me if you recognize it?");
                    dialogLines.Enqueue("Villager: Ah, this is no ordinary flower. It's a Helphinium - a poisonous species that's quite rare in these parts. I've studied them extensively during my botanist days. It's known for its toxic properties.");
                    dialogLines.Enqueue("Detective: Toxic properties, you say? How dangerous is it?");
                    dialogLines.Enqueue("Villager: Quite dangerous, Detective. In the wrong hands, it can be used to concoct a lethal potion. The toxins in this flower are potent enough to cause harm. You must be cautious with it.");
                    dialogLines.Enqueue("Detective: Walter, did Evelyn ever mention anything about this particular flower? Any reason she might have been interested in it?");
                    dialogLines.Enqueue("Villager: I can't say for certain if she was specifically interested in this flower, but we did have our disagreements about her experimenting with unusual blooms. If she was working with this one, it could explain some things. My concerns were always about the potential dangers these flowers posed to the village.");
                    dialogLines.Enqueue("Detective: So, you're saying Evelyn might have been using this flower in her experiments?");
                    dialogLines.Enqueue("Villager: It's a possibility, Detective. As much as I disapproved of her methods, I never thought she'd take it this far. If she was indeed using this poisonous flower, it adds another layer to the already complex situation.");
                    dialogLines.Enqueue("Detective: Thank you, Walter. This information is crucial to the investigation. If you remember anything else related to Evelyn's experiments or this flower, please let me know.");
                    dialogLines.Enqueue("Villager: Of course, Detective. I want to see justice served for Evelyn. If there's anything else I can do to help, don't hesitate to ask.");
                    dialogLines.Enqueue("System: clueB");
                }
                else if (playerStorage.cluesFound[5] && !playerStorage.dialogueRead[1].clueCFound) // Found potion
                {
                    dialogLines.Enqueue("Detective: Walter, I found this mysterious potion in an underground lair beneath the florist shop. Do you think this could've been used in the murder?");
                    dialogLines.Enqueue("Villager: Wow, Detective, if this potion is what I think it is, it is definitely derived from the poisonous Helphinium flower. Where did you find this?");
                    dialogLines.Enqueue("Detective: It was hidden beneath the floorboards in a secret room beneath Evelyn's shop. Do you have any idea how it got there?");
                    dialogLines.Enqueue("Villager: A secret room, you say? That's preposterous! I had no knowledge of such a place. But that potion, Detective, that's a concoction only a skilled botanist could create. It's made from the Helphinium flower, an invasive species I've warned Evelyn about in the past.");
                    dialogLines.Enqueue("Detective: Are you suggesting Evelyn might have been involved in making this potion?");
                    dialogLines.Enqueue("Villager: No, no, Detective. Evelyn was headstrong and had her quirks, but she would never dabble in something so dangerous. My guess is someone with botanical knowledge, like myself, concocted this potion. But I assure you, it wasn't me. I value life, especially in the realm of plants.");
                    dialogLines.Enqueue("Detective: Walter, the only other florist in this town is Lilian. Do you think she may be involved?");
                    dialogLines.Enqueue("Villager: Lilian, you say? Hmph. That's hard to believe. She was Evelyn's apprentice, after all. If she claims ignorance, she must be behind all of this. There's no way she didn't know about the potion or the secret room. She must be trying to throw you off her trail.");
                    dialogLines.Enqueue("Detective: Are you suggesting Lilian is the one who created and used this potion?");
                    dialogLines.Enqueue("Villager: I can't say for certain, Detective, but if she says she doesn't know, she's either lying or deeply involved. This potion is not something an amateur would concoct. She must have had help or guidance. Confront her, and you might find the answers you seek.");
                    dialogLines.Enqueue("System: clueC");
                }
                break;
            default:
                break;
        }
    }

    private void SetDialogAndLabel(string text)
    {
        // Grab everything before ": " and set it as the label
        // Grab everything after ": " and set it as the dialog
        string[] splitText = text.Split(new string[] { ": " }, System.StringSplitOptions.None);
        dialogManager.SetDialog(splitText[1]);
        if (splitText[0] == "Villager")
        {
            dialogManager.SetLabel(villagerName);
        }
        else
        {
            dialogManager.SetLabel(splitText[0]);
        }
    }
}
