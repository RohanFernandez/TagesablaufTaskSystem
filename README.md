# Tagesablauf Task System
A task system in Unity to create a sequence of tasks to execute and manage them.

## About
Executing a sequence looks like...
Task1 : Enable a gameobject
Task2 : Move the gameobject
Task3 : Wait for 10 seconds
Task4 : Log "I waited for 10 seconds"
Task5 : Disable the gameobject

You can create difference types of these tasks and execute them when you want through code.
You can create as many task lists as you need and set the task list you want to use at that time as the 'current task list'.
Start, stop and manage your system. I have used this system in a previously published game and created multiple of these task types as needed by game designers.
These work similar to story graphs and can be used by non-technical staff.

## Tutorial
1. Open the 'Example' scene
2. In the toolbar there exists an option 'Task System'->'Manager'. This opens the 'Manager' window to create tasklists or run sequences at runtime for debugging.
3. 'Task List XML Source Path' is the directory of the folder that includes all the task lists. A Task List is an XML list that includes your sequences and tasks. You can have as many of these task lists as you desire.
4. When you edit a task list, it needs to be turned into a Unity Asset. You can do this by clicking 'Create All Scriptable Objects'. These newly created Unity Assets are then placed into the folder in the directory 'Task List Asset Destination Path'
5. Once the game begins, add the task lists you need at that moment and you can select a single task list to be used as the current to refer to. This task list is the one you refer to when whish to execute a sequence i.e. the sequence you run should be found in this task list.
6. Currently there exists 2 example tasks: TaskWait and TaskLog. To create more types of tasks, replicate the .cs code and add your relevant addition in place of the inherited code.
    1. onInitialize() is called when the sequence begins executing.
    2. onExecute() Called when that task is executed
    3. onUpdate() Called while the task is ongoing similar to the update function but for that task specifically
    4. onComplete() Called when the task is completed. This needs to be called by you specifying that this task is completed. For example: TaskWait waits for some time and after that specified time calls onComplete()
7. The TaskManager manager all execution and management of tasks, call TaskManagerExecuteSequence(SequenceName) to start a sequence. The sequence can also be stopped etc.
