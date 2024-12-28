# How to Meet WCAG (Quickref Reference)
**Principle 1** – Perceivable
-----------------------------

Information and user interface components must be presentable to users in ways they can perceive.

### Guideline **1.1** – Text Alternatives

Provide text alternatives for any non-text content so that it can be changed into other forms people need, such as large print, braille, speech, symbols or simpler language.

#### **1.1.1** Non-text Content

Level A

All non-text content that is presented to the user has a text alternative that serves the equivalent purpose, except for the situations listed below.

* * *

*   **Controls, Input:** If non-text content is a control or accepts user input, then it has a name that describes its purpose. (Refer to Success Criterion 4.1.2 for additional requirements for controls and content that accepts user input.)
    
*   **Time-Based Media:** If non-text content is time-based media, then text alternatives at least provide descriptive identification of the non-text content. (Refer to Guideline 1.2 for additional requirements for media.)
    
*   **Test:** If non-text content is a test or exercise that would be invalid if presented in text, then text alternatives at least provide descriptive identification of the non-text content.
    
*   **Sensory:** If non-text content is primarily intended to create a specific sensory experience, then text alternatives at least provide descriptive identification of the non-text content.
    
*   **CAPTCHA:** If the purpose of non-text content is to confirm that content is being accessed by a person rather than a computer, then text alternatives that identify and describe the purpose of the non-text content are provided, and alternative forms of CAPTCHA using output modes for different types of sensory perception are provided to accommodate different disabilities.
    
*   **Decoration, Formatting, Invisible:** If non-text content is pure decoration, is used only for visual formatting, or is not presented to users, then it is implemented in a way that it can be ignored by assistive technology.
    

Understanding 1.1.1

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 1.1.1

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

###### Situation A: If a short description can serve the same purpose and present the same information as the non-text content:

###### Situation B: If a short description can not serve the same purpose and present the same information as the non-text content (e.g., a chart or diagram):

###### Situation C: If non-text content is a control or accepts user input:

###### Situation D: If non-text content is time-based media (including live video-only and live audio-only); a test or exercise that would be invalid if presented in text; or primarily intended to create a specific sensory experience:

###### Situation E: If non-text content is a CAPTCHA:

###### Situation F: If the non-text content should be ignored by assistive technology:

*   Implementing or marking the non-text content so that it will be ignored by assistive technology using one of the following techniques:
    
    Techniques to indicate that text alternatives are not required for Situation F:
    
    *   C9: Using CSS to include decorative images
    *   H67: Using null alt text and no title attribute on img elements for images that AT should ignore
    *   PDF4: Hiding decorative images with the Artifact tag in PDF documents

##### Advisory Techniques for Success Criterion 1.1.1

##### Failures for Success Criterion 1.1.1

### Guideline **1.2** – Time-based Media

Provide alternatives for time-based media.

#### **1.2.1** Audio-only and Video-only (Prerecorded)

Level A

For prerecorded audio-only and prerecorded video-only media, the following are true, except when the audio or video is a media alternative for text and is clearly labeled as such:

* * *

*   **Prerecorded Audio-only:** An alternative for time-based media is provided that presents equivalent information for prerecorded audio-only content.
    
*   **Prerecorded Video-only:** Either an alternative for time-based media or an audio track is provided that presents equivalent information for prerecorded video-only content.
    

Understanding 1.2.1

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 1.2.1

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

###### Situation A: If the content is prerecorded audio-only:

###### Situation B: If the content is prerecorded video-only:

##### Advisory Techniques for Success Criterion 1.2.1

##### Failures for Success Criterion 1.2.1

#### **1.2.2** Captions (Prerecorded)

Level A

Captions are provided for all prerecorded audio content in synchronized media, except when the media is a media alternative for text and is clearly labeled as such.

Understanding 1.2.2

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 1.2.2

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Failures for Success Criterion 1.2.2

#### **1.2.3** Audio Description or Media Alternative (Prerecorded)

Level A

An alternative for time-based media or audio description of the prerecorded video content is provided for synchronized media, except when the media is a media alternative for text and is clearly labeled as such.

Understanding 1.2.3

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 1.2.3

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Advisory Techniques for Success Criterion 1.2.3

#### **1.2.4** Captions (Live)

Level AA

Captions are provided for all live audio content in synchronized media.

Understanding 1.2.4

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 1.2.4

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

#### **1.2.5** Audio Description (Prerecorded)

Level AA

Audio description is provided for all prerecorded video content in synchronized media.

Understanding 1.2.5

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 1.2.5

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Advisory Techniques for Success Criterion 1.2.5

#### **1.2.6** Sign Language (Prerecorded)

Level AAA

Sign language interpretation is provided for all prerecorded audio content in synchronized media.

Understanding 1.2.6

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 1.2.6

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

#### **1.2.7** Extended Audio Description (Prerecorded)

Level AAA

Where pauses in foreground audio are insufficient to allow audio descriptions to convey the sense of the video, extended audio description is provided for all prerecorded video content in synchronized media.

Understanding 1.2.7

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 1.2.7

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Advisory Techniques for Success Criterion 1.2.7

#### **1.2.8** Media Alternative (Prerecorded)

Level AAA

An alternative for time-based media is provided for all prerecorded synchronized media and for all prerecorded video-only media.

Understanding 1.2.8

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 1.2.8

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

###### Situation A: If the content is prerecorded synchronized media:

###### Situation B: If the content is prerecorded video-only:

##### Failures for Success Criterion 1.2.8

#### **1.2.9** Audio-only (Live)

Level AAA

An alternative for time-based media that presents equivalent information for live audio-only content is provided.

Understanding 1.2.9

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 1.2.9

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

### Guideline **1.3** – Adaptable

Create content that can be presented in different ways (for example simpler layout) without losing information or structure.

#### **1.3.1** Info and Relationships

Level A

Information, structure, and relationships conveyed through presentation can be programmatically determined or are available in text.

Understanding 1.3.1

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 1.3.1

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

###### Situation A: The technology provides semantic structure to make information and relationships conveyed through presentation programmatically determinable:

###### Situation B: The technology in use does NOT provide the semantic structure to make the information and relationships conveyed through presentation programmatically determinable:

##### Advisory Techniques for Success Criterion 1.3.1

##### Failures for Success Criterion 1.3.1

#### **1.3.2** Meaningful Sequence

Level A

When the sequence in which content is presented affects its meaning, a correct reading sequence can be programmatically determined.

Understanding 1.3.2

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 1.3.2

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Failures for Success Criterion 1.3.2

#### **1.3.3** Sensory Characteristics

Level A

Instructions provided for understanding and operating content do not rely solely on sensory characteristics of components such as shape, color, size, visual location, orientation, or sound.

_Note 1:_ For requirements related to color, refer to Guideline 1.4.

Understanding 1.3.3

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 1.3.3

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Failures for Success Criterion 1.3.3

#### **1.3.4** Orientation

Level AA(Added in 2.1)

Content does not restrict its view and operation to a single display orientation, such as portrait or landscape, unless a specific display orientation is essential.

Understanding 1.3.4

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 1.3.4

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Failures for Success Criterion 1.3.4

#### **1.3.5** Identify Input Purpose

Level AA(Added in 2.1)

The purpose of each input field collecting information about the user can be programmatically determined when:

* * *

*   The input field serves a purpose identified in the Input Purposes for User Interface Components section; and
    
*   The content is implemented using technologies with support for identifying the expected meaning for form input data.
    

Understanding 1.3.5

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 1.3.5

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Failures for Success Criterion 1.3.5

#### **1.3.6** Identify Purpose

Level AAA(Added in 2.1)

In content implemented using markup languages, the purpose of user interface components, icons, and regions can be programmatically determined.

Understanding 1.3.6

Sufficient Advisory Failures

##### Advisory Techniques for Success Criterion 1.3.6

*   Enabling user agents to find the version of the content that best fits their needs
*   Using semantics to identify important features (e.g., coga-simplification="simplest")
*   Using aria-invalid and aria-required

### Guideline **1.4** – Distinguishable

Make it easier for users to see and hear content including separating foreground from background.

#### **1.4.1** Use of Color

Level A

Color is not used as the only visual means of conveying information, indicating an action, prompting a response, or distinguishing a visual element.

_Note 1:_ This success criterion addresses color perception specifically. Other forms of perception are covered in Guideline 1.3 including programmatic access to color and other visual presentation coding.

Understanding 1.4.1

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 1.4.1

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

###### Situation A: If the color of particular words, backgrounds, or other content is used to indicate information:

###### Situation B: If color is used within an image to convey information:

##### Advisory Techniques for Success Criterion 1.4.1

##### Failures for Success Criterion 1.4.1

#### **1.4.2** Audio Control

Level A

If any audio on a Web page plays automatically for more than 3 seconds, either a mechanism is available to pause or stop the audio, or a mechanism is available to control audio volume independently from the overall system volume level.

Understanding 1.4.2

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 1.4.2

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Failures for Success Criterion 1.4.2

#### **1.4.3** Contrast (Minimum)

Level AA

The visual presentation of text and images of text has a contrast ratio of at least 4.5:1, except for the following:

* * *

*   **Large Text:** Large-scale text and images of large-scale text have a contrast ratio of at least 3:1;
    
*   **Incidental:** Text or images of text that are part of an inactive user interface component, that are pure decoration, that are not visible to anyone, or that are part of a picture that contains significant other visual content, have no contrast requirement.
    
*   **Logotypes:** Text that is part of a logo or brand name has no contrast requirement.
    

Understanding 1.4.3

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 1.4.3

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

###### Situation A: text is less than 18 point if not bold and less than 14 point if bold

###### Situation B: text is at least 18 point if not bold and at least 14 point if bold

##### Advisory Techniques for Success Criterion 1.4.3

##### Failures for Success Criterion 1.4.3

#### **1.4.4** Resize Text

Level AA

Except for captions and images of text, text can be resized without assistive technology up to 200 percent without loss of content or functionality.

Understanding 1.4.4

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 1.4.4

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Advisory Techniques for Success Criterion 1.4.4

##### Failures for Success Criterion 1.4.4

#### **1.4.5** Images of Text

Level AA

If the technologies being used can achieve the visual presentation, text is used to convey information rather than images of text except for the following:

* * *

*   **Customizable:** The image of text can be visually customized to the user's requirements;
    
*   **Essential:** A particular presentation of text is essential to the information being conveyed.
    

_Note 1:_ Logotypes (text that is part of a logo or brand name) are considered essential.

Understanding 1.4.5

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 1.4.5

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Advisory Techniques for Success Criterion 1.4.5

#### **1.4.6** Contrast (Enhanced)

Level AAA

The visual presentation of text and images of text has a contrast ratio of at least 7:1, except for the following:

* * *

*   **Large Text:** Large-scale text and images of large-scale text have a contrast ratio of at least 4.5:1;
    
*   **Incidental:** Text or images of text that are part of an inactive user interface component, that are pure decoration, that are not visible to anyone, or that are part of a picture that contains significant other visual content, have no contrast requirement.
    
*   **Logotypes:** Text that is part of a logo or brand name has no contrast requirement.
    

Understanding 1.4.6

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 1.4.6

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

###### Situation A: text is less than 18 point if not bold and less than 14 point if bold

###### Situation B: text is as least 18 point if not bold and at least 14 point if bold

##### Advisory Techniques for Success Criterion 1.4.6

##### Failures for Success Criterion 1.4.6

#### **1.4.7** Low or No Background Audio

Level AAA

For prerecorded audio-only content that (1) contains primarily speech in the foreground, (2) is not an audio CAPTCHA or audio logo, and (3) is not vocalization intended to be primarily musical expression such as singing or rapping, at least one of the following is true:

* * *

*   **No Background:** The audio does not contain background sounds.
    
*   **Turn Off:** The background sounds can be turned off.
    
*   **20 dB:** The background sounds are at least 20 decibels lower than the foreground speech content, with the exception of occasional sounds that last for only one or two seconds. Per the definition of "decibel," background sound that meets this requirement will be approximately four times quieter than the foreground speech content.
    

Understanding 1.4.7

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 1.4.7

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

#### **1.4.8** Visual Presentation

Level AAA

For the visual presentation of blocks of text, a mechanism is available to achieve the following:

* * *

1.  Foreground and background colors can be selected by the user.
    
2.  Width is no more than 80 characters or glyphs (40 if CJK).
    
3.  Text is not justified (aligned to both the left and the right margins).
    
4.  Line spacing (leading) is at least space-and-a-half within paragraphs, and paragraph spacing is at least 1.5 times larger than the line spacing.
    
5.  Text can be resized without assistive technology up to 200 percent in a way that does not require the user to scroll horizontally to read a line of text on a full-screen window.
    

Understanding 1.4.8

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 1.4.8

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

###### First Requirement: Techniques to ensure foreground and background colors can be selected by the user

###### Second Requirement: Techniques to ensure width is no more than 80 characters or glyphs (40 if CJK)

###### Third Requirement: Techniques to ensure text is not justified (aligned to both the left and the right margins)

###### Fourth Requirement: Techniques to ensure line spacing (leading) is at least space-and-a-half within paragraphs, and paragraph spacing is at least 1.5 times larger than the line spacing

###### Fifth Requirement: Techniques to ensure text can be resized without assistive technology up to 200 percent in a way that does not require the user to scroll horizontally to read a line of text on a full-screen window

##### Failures for Success Criterion 1.4.8

#### **1.4.9** Images of Text (No Exception)

Level AAA

Images of text are only used for pure decoration or where a particular presentation of text is essential to the information being conveyed.

_Note 1:_ Logotypes (text that is part of a logo or brand name) are considered essential.

Understanding 1.4.9

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 1.4.9

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Advisory Techniques for Success Criterion 1.4.9

#### **1.4.10** Reflow

Level AA(Added in 2.1)

Content can be presented without loss of information or functionality, and without requiring scrolling in two dimensions for:

* * *

*   Vertical scrolling content at a width equivalent to 320 CSS pixels;
    
*   Horizontal scrolling content at a height equivalent to 256 CSS pixels;
    

Except for parts of the content which require two-dimensional layout for usage or meaning.

Understanding 1.4.10

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 1.4.10

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Failures for Success Criterion 1.4.10

#### **1.4.11** Non-text Contrast

Level AA(Added in 2.1)

The visual presentation of the following have a contrast ratio of at least 3:1 against adjacent color(s):

* * *

*   **User Interface Components:** Visual information required to identify user interface components and states, except for inactive components or where the appearance of the component is determined by the user agent and not modified by the author;
    
*   **Graphical Objects:** Parts of graphics required to understand the content, except when a particular presentation of graphics is essential to the information being conveyed.
    

Understanding 1.4.11

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 1.4.11

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

###### Situation A: Color is used to identify user interface components or used to identify user interface component states

###### Situation B: Color is required to understand graphical content

##### Failures for Success Criterion 1.4.11

#### **1.4.12** Text Spacing

Level AA(Added in 2.1)

In content implemented using markup languages that support the following text style properties, no loss of content or functionality occurs by setting all of the following and by changing no other style property:

* * *

*   Line height (line spacing) to at least 1.5 times the font size;
    
*   Spacing following paragraphs to at least 2 times the font size;
    
*   Letter spacing (tracking) to at least 0.12 times the font size;
    
*   Word spacing to at least 0.16 times the font size.
    

Exception: Human languages and scripts that do not make use of one or more of these text style properties in written text can conform using only the properties that exist for that combination of language and script.

Understanding 1.4.12

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 1.4.12

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Advisory Techniques for Success Criterion 1.4.12

##### Failures for Success Criterion 1.4.12

#### **1.4.13** Content on Hover or Focus

Level AA(Added in 2.1)

Where receiving and then removing pointer hover or keyboard focus triggers additional content to become visible and then hidden, the following are true:

* * *

*   **Dismissible:** A mechanism is available to dismiss the additional content without moving pointer hover or keyboard focus, unless the additional content communicates an input error or does not obscure or replace other content;
    
*   **Hoverable:** If pointer hover can trigger the additional content, then the pointer can be moved over the additional content without the additional content disappearing;
    
*   **Persistent:** The additional content remains visible until the hover or focus trigger is removed, the user dismisses it, or its information is no longer valid.
    

Exception: The visual presentation of the additional content is controlled by the user agent and is not modified by the author.

Understanding 1.4.13

Sufficient Advisory Failures

**Principle 2** – Operable
--------------------------

User interface components and navigation must be operable.

### Guideline **2.1** – Keyboard Accessible

Make all functionality available from a keyboard.

#### **2.1.1** Keyboard

Level A

All functionality of the content is operable through a keyboard interface without requiring specific timings for individual keystrokes, except where the underlying function requires input that depends on the path of the user's movement and not just the endpoints.

_Note 1:_ This exception relates to the underlying function, not the input technique. For example, if using handwriting to enter text, the input technique (handwriting) requires path-dependent input but the underlying function (text input) does not.

_Note 2:_ This does not forbid and should not discourage providing mouse input or other input methods in addition to keyboard operation.

Understanding 2.1.1

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 2.1.1

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Advisory Techniques for Success Criterion 2.1.1

##### Failures for Success Criterion 2.1.1

#### **2.1.2** No Keyboard Trap

Level A

If keyboard focus can be moved to a component of the page using a keyboard interface, then focus can be moved away from that component using only a keyboard interface, and, if it requires more than unmodified arrow or tab keys or other standard exit methods, the user is advised of the method for moving focus away.

_Note 1:_ Since any content that does not meet this success criterion can interfere with a user's ability to use the whole page, all content on the Web page (whether it is used to meet other success criteria or not) must meet this success criterion. See Conformance Requirement 5: Non-Interference.

Understanding 2.1.2

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 2.1.2

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Failures for Success Criterion 2.1.2

#### **2.1.3** Keyboard (No Exception)

Level AAA

All functionality of the content is operable through a keyboard interface without requiring specific timings for individual keystrokes.

Understanding 2.1.3

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 2.1.3

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

*   No additional techniques exist for this Success Criterion. Follow techniques for Success Criterion 2.1.1. If that is not possible because there is a requirement for path-dependent input, then it is not possible to meet this Level AAA Success Criterion.

#### **2.1.4** Character Key Shortcuts

Level A(Added in 2.1)

If a keyboard shortcut is implemented in content using only letter (including upper- and lower-case letters), punctuation, number, or symbol characters, then at least one of the following is true:

* * *

*   **Turn off:** A mechanism is available to turn the shortcut off;
    
*   **Remap:** A mechanism is available to remap the shortcut to include one or more non-printable keyboard keys (e.g., Ctrl, Alt);
    
*   **Active only on focus:** The keyboard shortcut for a user interface component is only active when that component has focus.
    

Understanding 2.1.4

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 2.1.4

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Failures for Success Criterion 2.1.4

### Guideline **2.2** – Enough Time

Provide users enough time to read and use content.

#### **2.2.1** Timing Adjustable

Level A

For each time limit that is set by the content, at least one of the following is true:

* * *

*   **Turn off:** The user is allowed to turn off the time limit before encountering it; or
    
*   **Adjust:** The user is allowed to adjust the time limit before encountering it over a wide range that is at least ten times the length of the default setting; or
    
*   **Extend:** The user is warned before time expires and given at least 20 seconds to extend the time limit with a simple action (for example, "press the space bar"), and the user is allowed to extend the time limit at least ten times; or
    
*   **Real-time Exception:** The time limit is a required part of a real-time event (for example, an auction), and no alternative to the time limit is possible; or
    
*   **Essential Exception:** The time limit is essential and extending it would invalidate the activity; or
    
*   **20 Hour Exception:** The time limit is longer than 20 hours.
    

Understanding 2.2.1

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 2.2.1

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

###### Situation A: If there are session time limits:

###### Situation B: If a time limit is controlled by a script on the page:

###### Situation C: If there are time limits on reading:

##### Failures for Success Criterion 2.2.1

#### **2.2.2** Pause, Stop, Hide

Level A

For moving, blinking, scrolling, or auto-updating information, all of the following are true:

* * *

*   **Moving, blinking, scrolling:** For any moving, blinking or scrolling information that (1) starts automatically, (2) lasts more than five seconds, and (3) is presented in parallel with other content, there is a mechanism for the user to pause, stop, or hide it unless the movement, blinking, or scrolling is part of an activity where it is essential; and
    
*   **Auto-updating:** For any auto-updating information that (1) starts automatically and (2) is presented in parallel with other content, there is a mechanism for the user to pause, stop, or hide it or to control the frequency of the update unless the auto-updating is part of an activity where it is essential.
    

_Note 1:_ For requirements related to flickering or flashing content, refer to Guideline 2.3.

_Note 2:_ Since any content that does not meet this success criterion can interfere with a user's ability to use the whole page, all content on the Web page (whether it is used to meet other success criteria or not) must meet this success criterion. See Conformance Requirement 5: Non-Interference.

_Note 3:_ Content that is updated periodically by software or that is streamed to the user agent is not required to preserve or present information that is generated or received between the initiation of the pause and resuming presentation, as this may not be technically possible, and in many situations could be misleading to do so.

_Note 4:_ An animation that occurs as part of a preload phase or similar situation can be considered essential if interaction cannot occur during that phase for all users and if not indicating progress could confuse users or cause them to think that content was frozen or broken.

Understanding 2.2.2

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 2.2.2

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Failures for Success Criterion 2.2.2

#### **2.2.3** No Timing

Level AAA

Timing is not an essential part of the event or activity presented by the content, except for non-interactive synchronized media and real-time events.

Understanding 2.2.3

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 2.2.3

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

#### **2.2.4** Interruptions

Level AAA

Interruptions can be postponed or suppressed by the user, except interruptions involving an emergency.

Understanding 2.2.4

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 2.2.4

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Failures for Success Criterion 2.2.4

#### **2.2.5** Re-authenticating

Level AAA

When an authenticated session expires, the user can continue the activity without loss of data after re-authenticating.

Understanding 2.2.5

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 2.2.5

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Failures for Success Criterion 2.2.5

#### **2.2.6** Timeouts

Level AAA(Added in 2.1)

Users are warned of the duration of any user inactivity that could cause data loss, unless the data is preserved for more than 20 hours when the user does not take any actions.

Understanding 2.2.6

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 2.2.6

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

*   Setting a session timeout to occur following at least 20 hours of inactivity.
*   Store user data for more than 20 hours.
*   Provide a warning of the duration of user inactivity at the start of a process.

### Guideline **2.3** – Seizures and Physical Reactions

Do not design content in a way that is known to cause seizures or physical reactions.

#### **2.3.1** Three Flashes or Below Threshold

Level A

Web pages do not contain anything that flashes more than three times in any one second period, or the flash is below the general flash and red flash thresholds.

_Note 1:_ Since any content that does not meet this success criterion can interfere with a user's ability to use the whole page, all content on the Web page (whether it is used to meet other success criteria or not) must meet this success criterion. See Conformance Requirement 5: Non-Interference.

Understanding 2.3.1

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 2.3.1

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

#### **2.3.2** Three Flashes

Level AAA

Web pages do not contain anything that flashes more than three times in any one second period.

Understanding 2.3.2

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 2.3.2

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

#### **2.3.3** Animation from Interactions

Level AAA(Added in 2.1)

Motion animation triggered by interaction can be disabled, unless the animation is essential to the functionality or the information being conveyed.

Understanding 2.3.3

Sufficient Advisory Failures

### Guideline **2.4** – Navigable

Provide ways to help users navigate, find content, and determine where they are.

#### **2.4.1** Bypass Blocks

Level A

A mechanism is available to bypass blocks of content that are repeated on multiple Web pages.

Understanding 2.4.1

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 2.4.1

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Advisory Techniques for Success Criterion 2.4.1

#### **2.4.2** Page Titled

Level A

Web pages have titles that describe topic or purpose.

Understanding 2.4.2

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 2.4.2

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Advisory Techniques for Success Criterion 2.4.2

##### Failures for Success Criterion 2.4.2

#### **2.4.3** Focus Order

Level A

If a Web page can be navigated sequentially and the navigation sequences affect meaning or operation, focusable components receive focus in an order that preserves meaning and operability.

Understanding 2.4.3

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 2.4.3

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Failures for Success Criterion 2.4.3

#### **2.4.4** Link Purpose (In Context)

Level A

The purpose of each link can be determined from the link text alone or from the link text together with its programmatically determined link context, except where the purpose of the link would be ambiguous to users in general.

Understanding 2.4.4

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 2.4.4

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Advisory Techniques for Success Criterion 2.4.4

##### Failures for Success Criterion 2.4.4

#### **2.4.5** Multiple Ways

Level AA

More than one way is available to locate a Web page within a set of Web pages except where the Web Page is the result of, or a step in, a process.

Understanding 2.4.5

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 2.4.5

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Advisory Techniques for Success Criterion 2.4.5

#### **2.4.6** Headings and Labels

Level AA

Headings and labels describe topic or purpose.

Understanding 2.4.6

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 2.4.6

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

#### **2.4.7** Focus Visible

Level AA

Any keyboard operable user interface has a mode of operation where the keyboard focus indicator is visible.

Understanding 2.4.7

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 2.4.7

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Failures for Success Criterion 2.4.7

#### **2.4.8** Location

Level AAA

Information about the user's location within a set of Web pages is available.

Understanding 2.4.8

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 2.4.8

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Advisory Techniques for Success Criterion 2.4.8

#### **2.4.9** Link Purpose (Link Only)

Level AAA

A mechanism is available to allow the purpose of each link to be identified from link text alone, except where the purpose of the link would be ambiguous to users in general.

Understanding 2.4.9

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 2.4.9

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Advisory Techniques for Success Criterion 2.4.9

##### Failures for Success Criterion 2.4.9

#### **2.4.10** Section Headings

Level AAA

Section headings are used to organize the content.

_Note 1:_ "Heading" is used in its general sense and includes titles and other ways to add a heading to different types of content.

_Note 2:_ This success criterion covers sections within writing, not user interface components. User Interface components are covered under Success Criterion 4.1.2.

Understanding 2.4.10

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 2.4.10

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

#### **2.4.11** Focus Not Obscured (Minimum)

Level AA(Added in 2.2)

When a user interface component receives keyboard focus, the component is not entirely hidden due to author-created content.

_Note 1:_ Where content in a configurable interface can be repositioned by the user, then only the initial positions of user-movable content are considered for testing and conformance of this Success Criterion.

_Note 2:_ Content opened by the user may obscure the component receiving focus. If the user can reveal the focused component without advancing the keyboard focus, the component with focus is not considered hidden due to author-created content.

Understanding 2.4.11

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 2.4.11

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Failures for Success Criterion 2.4.11

#### **2.4.12** Focus Not Obscured (Enhanced)

Level AAA(Added in 2.2)

When a user interface component receives keyboard focus, no part of the component is hidden by author-created content.

Understanding 2.4.12

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 2.4.12

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Failures for Success Criterion 2.4.12

*   An interaction that causes content to appear over the component with keyboard focus, visually covering part of the focus indicator. This behavior might be encountered with advertising or promotional material meant to provide more information about a product as the user navigates through a catalogue.
*   A page has a sticky footer (attached to the bottom of the viewport). When tabbing down the page, a focused item is partially hidden by the footer because content in the viewport scrolls without sufficient scroll padding.

#### **2.4.13** Focus Appearance

Level AAA(Added in 2.2)

When the keyboard focus indicator is visible, an area of the focus indicator meets all the following:

* * *

*   is at least as large as the area of a 2 CSS pixel thick perimeter of the unfocused component or sub-component, and
    
*   has a contrast ratio of at least 3:1 between the same pixels in the focused and unfocused states.
    

Exceptions:

* * *

*   The focus indicator is determined by the user agent and cannot be adjusted by the author, or
    
*   The focus indicator and the indicator’s background color are not modified by the author.
    

_Note 1:_ What is perceived as the user interface component or sub-component (to determine enclosure or size) depends on its visual presentation. The visual presentation includes the component's visible content, border, and component-specific background. It does not include shadow and glow effects outside the component's content, background, or border.

_Note 2:_ Examples of sub-components that may receive a focus indicator are menu items in an opened drop-down menu, or focusable cells in a grid.

_Note 3:_ Contrast calculations can be based on colors defined within the technology (such as HTML, CSS and SVG). Pixels modified by user agent resolution enhancements and anti-aliasing can be ignored.

Understanding 2.4.13

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 2.4.13

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Failures for Success Criterion 2.4.13

*   Using a CSS border for inline text which can wrap (Potential future technique)

### Guideline **2.5** – Input Modalities

Make it easier for users to operate functionality through various inputs beyond keyboard.

#### **2.5.1** Pointer Gestures

Level A(Added in 2.1)

All functionality that uses multipoint or path-based gestures for operation can be operated with a single pointer without a path-based gesture, unless a multipoint or path-based gesture is essential.

Understanding 2.5.1

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 2.5.1

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Failures for Success Criterion 2.5.1

#### **2.5.2** Pointer Cancellation

Level A(Added in 2.1)

For functionality that can be operated using a single pointer, at least one of the following is true:

* * *

*   **No Down-Event:** The down-event of the pointer is not used to execute any part of the function;
    
*   **Abort or Undo:** Completion of the function is on the up-event, and a mechanism is available to abort the function before completion or to undo the function after completion;
    
*   **Up Reversal:** The up-event reverses any outcome of the preceding down-event;
    
*   **Essential:** Completing the function on the down-event is essential.
    

Understanding 2.5.2

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 2.5.2

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Failures for Success Criterion 2.5.2

#### **2.5.3** Label in Name

Level A(Added in 2.1)

For user interface components with labels that include text or images of text, the name contains the text that is presented visually.

Understanding 2.5.3

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 2.5.3

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Advisory Techniques for Success Criterion 2.5.3

*   If an icon has no accompanying text, consider using its hover text as its accessible name (Potential future technique)

##### Failures for Success Criterion 2.5.3

*   F96: Failure due to the accessible name not containing the visible label text
*   Accessible name contains the visible label text, but the words of the visible label are not in the same order as they are in the visible label text (Potential future technique)
*   Accessible name contains the visible label text, but one or more other words are interspersed in the label (Potential future technique)

#### **2.5.4** Motion Actuation

Level A(Added in 2.1)

Functionality that can be operated by device motion or user motion can also be operated by user interface components and responding to the motion can be disabled to prevent accidental actuation, except when:

* * *

*   **Supported Interface:** The motion is used to operate functionality through an accessibility supported interface;
    
*   **Essential:** The motion is essential for the function and doing so would invalidate the activity.
    

Understanding 2.5.4

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 2.5.4

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

#### **2.5.5** Target Size (Enhanced)

Level AAA(Added in 2.1)

The size of the target for pointer inputs is at least 44 by 44 CSS pixels except when:

* * *

*   **Equivalent:** The target is available through an equivalent link or control on the same page that is at least 44 by 44 CSS pixels;
    
*   **Inline:** The target is in a sentence or block of text;
    
*   **User Agent Control:** The size of the target is determined by the user agent and is not modified by the author;
    
*   **Essential:** A particular presentation of the target is essential to the information being conveyed.
    

Understanding 2.5.5

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 2.5.5

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

*   Ensuring that targets are at least 44 by 44 CSS pixels.
*   Ensuring inline links provide sufficiently large activation target.

##### Advisory Techniques for Success Criterion 2.5.5

*   none documented

##### Failures for Success Criterion 2.5.5

*   Failure of success criterion 2.5.5 due to target being less than 44 by 44 CSS pixels.

#### **2.5.6** Concurrent Input Mechanisms

Level AAA(Added in 2.1)

Web content does not restrict use of input modalities available on a platform except where the restriction is essential, required to ensure the security of the content, or required to respect user settings.

Understanding 2.5.6

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 2.5.6

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

*   Only using high-level, input-agnostic event handlers, such as focus, blur, click, in Javascript (Potential future technique).

##### Failures for Success Criterion 2.5.6

#### **2.5.7** Dragging Movements

Level AA(Added in 2.2)

All functionality that uses a dragging movement for operation can be achieved by a single pointer without dragging, unless dragging is essential or the functionality is determined by the user agent and not modified by the author.

_Note:_ This requirement applies to web content that interprets pointer actions (i.e. this does not apply to actions that are required to operate the user agent or assistive technology).

Understanding 2.5.7

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 2.5.7

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Failures for Success Criterion 2.5.7

#### **2.5.8** Target Size (Minimum)

Level AA(Added in 2.2)

The size of the target for pointer inputs is at least 24 by 24 CSS pixels, except where:

* * *

*   **Spacing:** Undersized targets (those less than 24 by 24 CSS pixels) are positioned so that if a 24 CSS pixel diameter circle is centered on the bounding box of each, the circles do not intersect another target or the circle for another undersized target;
    
*   **Equivalent:** The function can be achieved through a different control on the same page that meets this criterion;
    
*   **Inline:** The target is in a sentence or its size is otherwise constrained by the line-height of non-target text;
    
*   **User agent control:** The size of the target is determined by the user agent and is not modified by the author;
    
*   **Essential:** A particular presentation of the target is essential or is legally required for the information being conveyed.
    

_Note 1:_ Targets that allow for values to be selected spatially based on position within the target are considered one target for the purpose of the success criterion. Examples include sliders, color pickers displaying a gradient of colors, or editable areas where you position the cursor.

_Note 2:_ For inline targets the line-height should be interpreted as perpendicular to the flow of text. For example, in a language displayed vertically, the line-height would be horizontal.

Understanding 2.5.8

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 2.5.8

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

**Principle 3** – Understandable
--------------------------------

Information and the operation of the user interface must be understandable.

### Guideline **3.1** – Readable

Make text content readable and understandable.

#### **3.1.1** Language of Page

Level A

The default human language of each Web page can be programmatically determined.

Understanding 3.1.1

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 3.1.1

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Advisory Techniques for Success Criterion 3.1.1

#### **3.1.2** Language of Parts

Level AA

The human language of each passage or phrase in the content can be programmatically determined except for proper names, technical terms, words of indeterminate language, and words or phrases that have become part of the vernacular of the immediately surrounding text.

Understanding 3.1.2

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 3.1.2

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

#### **3.1.3** Unusual Words

Level AAA

A mechanism is available for identifying specific definitions of words or phrases used in an unusual or restricted way, including idioms and jargon.

Understanding 3.1.3

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 3.1.3

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

###### Situation A: If the word or phrase has a unique meaning within the Web page:

###### Situation B: If the word or phrase means different things within the same Web page:

#### **3.1.4** Abbreviations

Level AAA

A mechanism for identifying the expanded form or meaning of abbreviations is available.

Understanding 3.1.4

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 3.1.4

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

###### Situation A: If the abbreviation has only one meaning within the Web page:

###### Situation B: If the abbreviation means different things within the same Web page:

#### **3.1.5** Reading Level

Level AAA

When text requires reading ability more advanced than the lower secondary education level after removal of proper names and titles, supplemental content, or a version that does not require reading ability more advanced than the lower secondary education level, is available.

Understanding 3.1.5

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 3.1.5

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

#### **3.1.6** Pronunciation

Level AAA

A mechanism is available for identifying specific pronunciation of words where meaning of the words, in context, is ambiguous without knowing the pronunciation.

Understanding 3.1.6

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 3.1.6

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

### Guideline **3.2** – Predictable

Make Web pages appear and operate in predictable ways.

#### **3.2.1** On Focus

Level A

When any user interface component receives focus, it does not initiate a change of context.

Understanding 3.2.1

Sufficient Advisory Failures

##### Advisory Techniques for Success Criterion 3.2.1

##### Failures for Success Criterion 3.2.1

#### **3.2.2** On Input

Level A

Changing the setting of any user interface component does not automatically cause a change of context unless the user has been advised of the behavior before using the component.

Understanding 3.2.2

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 3.2.2

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Advisory Techniques for Success Criterion 3.2.2

##### Failures for Success Criterion 3.2.2

#### **3.2.3** Consistent Navigation

Level AA

Navigational mechanisms that are repeated on multiple Web pages within a set of Web pages occur in the same relative order each time they are repeated, unless a change is initiated by the user.

Understanding 3.2.3

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 3.2.3

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Advisory Techniques for Success Criterion 3.2.3

##### Failures for Success Criterion 3.2.3

#### **3.2.4** Consistent Identification

Level AA

Components that have the same functionality within a set of Web pages are identified consistently.

Understanding 3.2.4

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 3.2.4

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

*   G197: Using labels, names, and text alternatives consistently for content that has the same functionality

*   _Note 1:_ Text alternatives that are “consistent” are not always “identical.” For instance, you may have a graphical arrow at the bottom of a Web page that links to the next Web page. The text alternative may say “Go to page 4.” Naturally, it would not be appropriate to repeat this exact text alternative on the next Web page. It would be more appropriate to say “Go to page 5”. Although these text alternatives would not be identical, they would be consistent, and therefore would satisfy this Success Criterion.
*   _Note 2:_ A single non-text-content-item may be used to serve different functions. In such cases, different text alternatives are necessary and should be used. Examples can be commonly found with the use of icons such as check marks, cross marks, and traffic signs. Their functions can be different depending on the context of the Web page. A check mark icon may function as “approved”, “completed”, or “included”, to name a few, depending on the situation. Using “check mark” as text alternative across all Web pages does not help users understand the function of the icon. Different text alternatives can be used when the same non-text content serves multiple functions.

##### Failures for Success Criterion 3.2.4

#### **3.2.5** Change on Request

Level AAA

Changes of context are initiated only by user request or a mechanism is available to turn off such changes.

Understanding 3.2.5

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 3.2.5

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

###### Situation A: If the Web page allows automatic updates:

###### Situation B: If automatic redirects are possible:

###### Situation C: If the Web page uses pop-up windows:

###### Situation D: If using an onchange event on a select element:

##### Advisory Techniques for Success Criterion 3.2.5

##### Failures for Success Criterion 3.2.5

#### **3.2.6** Consistent Help

Level A(Added in 2.2)

If a Web page contains any of the following help mechanisms, and those mechanisms are repeated on multiple Web pages within a set of Web pages, they occur in the same order relative to other page content, unless a change is initiated by the user:

* * *

*   Human contact details;
    
*   Human contact mechanism;
    
*   Self-help option;
    
*   A fully automated contact mechanism.
    

_Note 1:_ A fully automated contact mechanism.

_Note 2:_ For this Success Criterion, “the same order relative to other page content” can be thought of as how the content is ordered when the page is serialized. The visual position of a help mechanism is likely to be consistent across pages for the same page variation (e.g., CSS break-point). The user can initiate a change, such as changing the page’s zoom or orientation, which may trigger a different page variation. This criterion is concerned with relative order across pages displayed in the same page variation (e.g., same zoom level and orientation).

Understanding 3.2.6

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 3.2.6

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Failures for Success Criterion 3.2.6

*   Inconsistent Help Location

### Guideline **3.3** – Input Assistance

Help users avoid and correct mistakes.

#### **3.3.1** Error Identification

Level A

If an input error is automatically detected, the item that is in error is identified and the error is described to the user in text.

Understanding 3.3.1

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 3.3.1

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

###### Situation A: If a form contains fields for which information from the user is mandatory.

###### Situation B: If information provided by the user is required to be in a specific data format or of certain values.

##### Advisory Techniques for Success Criterion 3.3.1

#### **3.3.2** Labels or Instructions

Level A

Labels or instructions are provided when content requires user input.

Understanding 3.3.2

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 3.3.2

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Advisory Techniques for Success Criterion 3.3.2

##### Failures for Success Criterion 3.3.2

#### **3.3.3** Error Suggestion

Level AA

If an input error is automatically detected and suggestions for correction are known, then the suggestions are provided to the user, unless it would jeopardize the security or purpose of the content.

Understanding 3.3.3

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 3.3.3

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

###### Situation A: If a mandatory field contains no information:

###### Situation B: If information for a field is required to be in a specific data format:

###### Situation C: Information provided by the user is required to be one of a limited set of values:

##### Advisory Techniques for Success Criterion 3.3.3

#### **3.3.4** Error Prevention (Legal, Financial, Data)

Level AA

For Web pages that cause legal commitments or financial transactions for the user to occur, that modify or delete user-controllable data in data storage systems, or that submit user test responses, at least one of the following is true:

* * *

*   **Reversible:** Submissions are reversible.
    
*   **Checked:** Data entered by the user is checked for input errors and the user is provided an opportunity to correct them.
    
*   **Confirmed:** A mechanism is available for reviewing, confirming, and correcting information before finalizing the submission.
    

Understanding 3.3.4

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 3.3.4

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

###### Situation A: If an application causes a legal transaction to occur, such as making a purchase or submitting an income tax return:

###### Situation B: If an action causes information to be deleted:

###### Situation C: If the Web page includes a testing application:

##### Advisory Techniques for Success Criterion 3.3.4

#### **3.3.5** Help

Level AAA

Context-sensitive help is available.

Understanding 3.3.5

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 3.3.5

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

###### Situation A: If a form requires text input:

###### Situation B: If a form requires text input in an expected data format:

##### Advisory Techniques for Success Criterion 3.3.5

#### **3.3.6** Error Prevention (All)

Level AAA

For Web pages that require the user to submit information, at least one of the following is true:

* * *

*   **Reversible:** Submissions are reversible.
    
*   **Checked:** Data entered by the user is checked for input errors and the user is provided an opportunity to correct them.
    
*   **Confirmed:** A mechanism is available for reviewing, confirming, and correcting information before finalizing the submission.
    

Understanding 3.3.6

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 3.3.6

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

##### Failures for Success Criterion 3.3.6

*   Inconsistent Help Location

#### **3.3.7** Redundant Entry

Level A(Added in 2.2)

Information previously entered by or provided to the user that is required to be entered again in the same process is either:

* * *

*   auto-populated, or
    
*   available for the user to select.
    

Except when:

* * *

*   re-entering the information is essential,
    
*   the information is required to ensure the security of the content, or
    
*   previously entered information is no longer valid.
    

Understanding 3.3.7

Sufficient Advisory Failures

#### **3.3.8** Accessible Authentication (Minimum)

Level AA(Added in 2.2)

A cognitive function test (such as remembering a password or solving a puzzle) is not required for any step in an authentication process unless that step provides at least one of the following:

* * *

*   **Alternative:** Another authentication method that does not rely on a cognitive function test.
    
*   **Mechanism:** A mechanism is available to assist the user in completing the cognitive function test.
    
*   **Object Recognition:** The cognitive function test is to recognize objects.
    
*   **Personal Content:** The cognitive function test is to identify non-text content the user provided to the Web site.
    

_Note 1:_ What is perceived as the user interface component or sub-component (to determine enclosure or size) depends on its visual presentation. The visual presentation includes the component's visible content, border, and component-specific background. It does not include shadow and glow effects outside the component's content, background, or border.

_Note 2:_ Examples of mechanisms that satisfy this criterion include:

1.  support for password entry by password managers to reduce memory need, and
2.  copy and paste to reduce the cognitive burden of re-typing.

Understanding 3.3.8

Sufficient Advisory Failures

##### Failures for Success Criterion 3.3.8

#### **3.3.9** Accessible Authentication (Enhanced)

Level AAA(Added in 2.2)

A cognitive function test (such as remembering a password or solving a puzzle) is not required for any step in an authentication process unless that step provides at least one of the following:

* * *

*   **Alternative:** Another authentication method that does not rely on a cognitive function test.
    
*   **Mechanism:** A mechanism is available to assist the user in completing the cognitive function test.
    

Understanding 3.3.9

Sufficient Advisory Failures

##### Failures for Success Criterion 3.3.9

**Principle 4** – Robust
------------------------

Content must be robust enough that it can be interpreted by a wide variety of user agents, including assistive technologies.

### Guideline **4.1** – Compatible

Maximize compatibility with current and future user agents, including assistive technologies.

#### **4.1.1** Parsing

Level A

In content implemented using markup languages, elements have complete start and end tags, elements are nested according to their specifications, elements do not contain duplicate attributes, and any IDs are unique, except where the specifications allow these features.

_Note 1:_ This criterion has been removed from WCAG 2.2. In WCAG 2.1 and 2.0 this Success Criterion should be considered as always satisfied for any content using HTML or XML.

_Note 2:_ Since this criterion was written, the HTML Standard has adopted specific requirements governing how user agents must handle incomplete tags, incorrect element nesting, duplicate attributes, and non-unique IDs.

Although the HTML Standard treats some of these cases as non-conforming for authors, it is considered to "allow these features" for the purposes of this Success Criterion because the specification requires that user agents support handling these cases consistently. In practice, this criterion no longer provides any benefit to people with disabilities in itself.

Issues such as missing roles due to inappropriately nested elements or incorrect states or names due to a duplicate ID are covered by different Success Criteria and should be reported under those criteria rather than as issues with 4.1.1.

Understanding 4.1.1

#### **4.1.2** Name, Role, Value

Level A

For all user interface components (including but not limited to: form elements, links and components generated by scripts), the name and role can be programmatically determined; states, properties, and values that can be set by the user can be programmatically set; and notification of changes to these items is available to user agents, including assistive technologies.

_Note 1:_ This success criterion is primarily for Web authors who develop or script their own user interface components. For example, standard HTML controls already meet this success criterion when used according to specification.

Understanding 4.1.2

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 4.1.2

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

###### Situation A: If using a standard user interface component in a markup language (e.g., HTML):

###### Situation B: If using script or code to re-purpose a standard user interface component in a markup language:

*   Exposing the names and roles, allowing user-settable properties to be directly set, and providing notification of changes using one of the following techniques:
    *   ARIA16: Using aria-labelledby to provide a name for user interface controls

###### Situation C: If using a standard user interface component in a programming technology:

###### Situation D: If creating your own user interface component in a programming language:

##### Failures for Success Criterion 4.1.2

#### **4.1.3** Status Messages

Level AA(Added in 2.1)

In content implemented using markup languages, status messages can be programmatically determined through role or properties such that they can be presented to the user by assistive technologies without receiving focus.

Understanding 4.1.3

Sufficient Advisory Failures

##### Sufficient Techniques for Success Criterion 4.1.3

Note: Other techniques may also be sufficient if they meet the success criterion. See Understanding Techniques.

###### Situation A: If a status message advises on the success or results of an action, or the state of an application:

###### Situation B: If a status message conveys a suggestion, or a warning on the existence of an error:

###### Situation C: If a status message conveys information on the progress of a process:

##### Failures for Success Criterion 4.1.3

* * *