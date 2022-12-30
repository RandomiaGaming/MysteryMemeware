# Uninstall Instructions
So you ignored all the warnings and installed MysteryMemeware on your system and now you are hopelessly stuck listening to the same song and staring at the same jpeg of a ghost until the end of time. Well here are a few solutions that might work, however, you are still legally responsible for any and all damages to your system. The following is a set of recommendations only and is not a guarantee that you will be able to recover your data. MysteryMemeware is malware and cannot be removed without significant risk of failure and data loss.

# Method 1
Requires a version of MysteryMemeware with a built-in fail safe.

Some versions of MysteryMemeware come with a built-in fail safe. If your copy of MysteryMemeware has a fail safe you will see a small message in the bottom right corner of your screen instructing you to press the "X" key. If you see this message, hold down the "X" key until a black screen appears. This may take several seconds. Once your screen goes black you are safe to release the "X" key. Wait and eventually your system will restart. After restarting the uninstallation process is complete and you should be able to use your system again.

# Method 2
Requires a second system and a blank USB thumb drive with at least 8 gigabytes of storage.

This method requires you to create a Windows 10 installer. To do this download the latest Media Creation Tool from Microsoft at https://www.microsoft.com/en-us/software-download/windows10 by clicking the "download now" button under the "Create Windows 10 installation media" sections. Run the Media Creation Tool and follow Microsoft's instructions to create a bootable Windows 10 installer. Fully shut down the infected system by holding down the power button until you see a black screen. Then insert the USB flash drive you made. Turn on the infected system and press the appropriate key on your keyboard to enter the BIOS or boot loader. Then boot from USB. Once you have booted from the Windows 10 installer hold the shift key and press F10. This will open a command prompt window. Run the following command but replace the letter "C" in "C:\" with the drive letter of the infected drive "copy C:\MysteryMemeware\MysteryUninstaller.exe C:\MysteryMemeware.exe /y". Press the enter key to run this command. After running the command you should see the output "1 file(s) copied." If you see this message then you can power off the infected system and remove the USB flash drive. Finally power on the infected system. After booting up the infected system will load and then it will restart. After restarting the uninstallation process is complete and you should be able to use your system again.

Note: If you do not see a section titled "Create Windows 10 installation media" after clicking the above link you may need to try again on a Windows 10 system. This option is only available on Windows 10 systems. If you are following these steps on a Mac OS or Linux system you will need to download a Windows 10 ISO and flash it to your USB flash drive manually using a 3rd party tool.
Note: If you are unable to determine which drive is the infected drive try running the "dir" command from the command prompt in your windows 10 Installer and specify different drive letters until you find a drive with a MysteryMemeware folder. The command will look something like "dir /a C:\MysteryMemeware" and will output "File Not Found" until you find the correct drive. Make sure to include the "/a" switch otherwise you may not find the MysteryMemeware folder even if it is there because it may be a hidden system folder.

# Method 3
Requires a second system and a way to connect an extra hard drive.

To repair your system without a flash drive you will need to remove the infected system's hard drive and mount it to another system. Fully shut down both the infected system and the second system by holding down their respective power buttons until black screens appear on both. Then remove the boot drive from the infected system and connect it to the second system. Boot up the second system with the infected drive as a secondary drive. Navigate to the MysteryMemeware folder in the root of the infected drive and delete MysteryMemeware.exe. Then rename MysteryUninstaller.exe to MysteryMemeware.exe. After completing these steps shut down the second system and plug the infected drive back into the original system. Turn on the infected system and after booting the infected system will load for a long time while it uninstalls MysteryMemeware and finally it will restart. After restarting you should be able to use your system again as normal.

Note: MysteryMemeware will not spread to other hard drives even if you accidentally boot into the infected drive while attempting repairs.

Note: If you are unable to locate the infected drive make sure that hidden files and system files are both shown. To do this you will need to open file explorer and navigate to the view tab in the top middle of the window. From there you should be able to check the checkbox next to "Hidden Items". Next use the windows search bar to search for "File Explorer Options" and open the first search result. Click over to the view tab and scroll down until you see a checkbox labeled "hide protected operating system files" and uncheck that checkbox. If a warning popup appears select yes to confirm your previous selection. Then press okay to close the "File Explorer Options" window.

# FAQ
What about Windows 11?
Even if your system is running Windows 11 the steps shown above should still work. I recommend continuing to use a Windows 10 installer even for Windows 11 systems, however, the instructions above will most likely work with a Windows 11 installer as well.

Can I uninstall MysteryMemeware without a second system?
Unless your version of MysteryMemeware includes a fail safe you cannot uninstall it without a second computer as far as I am aware. Some systems may have recovery options within their BIOS which may be able to assist you, however, MysteryMemeware disables all recovery options within Windows 10's WinRE so there is no way to uninstall it without a second system.

I can't find the MysteryMemeware folder?
If you can't find the MysteryMemeware folder it may be on a different drive. Try checking other drive letters such as "E:\" or "F:\". The MysteryMemeware folder may also be hidden. Make sure to show hidden files and even show system folders. If you really cannot find the MysteryMemeware folder then your version of MysteryMemeware may have been customized to hide this folder.

I can't find MysteryUninstaller.exe?
If you cannot find MysteryInstaller.exe within the MysteryMemeware folder your version of MysteryMemeware may be too old to include an uninstaller or may be customized to not include this recovery program.

What if none of the above methods worked for me?
Sadly you may be out of luck, however, if you bring your system into a professional show them these instructions they may be able to help you get your system fixed.
