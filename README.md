# Mikrotik AddressList Manager
This tool will be useful for those SOHO admins who manage their Mikrotik's via Firewall Address Lists.

## Briefly
MIkrotik AddressList Manager connects to the Mikrotik device and gets a list of Firewall AddressList's from it, allowing you to enable and disable individual elements (only static).

If, for example, you have an AddressList 'INTERNET_ACCESS' with list of hosts from LAN who are allowed access to the Internet (and you have a firewall rule configured accordingly for the corresponding list), then by enabling / disabling elements of this list through this program you will allow or block access to the corresponding resources for the specified hosts.

## Sensitive Data
The program stores a list of Mikrotik devices and their credentials (Address Book) in a roaming part of the user profile. This data is encrypted using a user-specified master password.

In an open plain text (unencrypted) form are stored the address of the last selected Mikrotik device and the collapsed/expanded statuses of the AddressList groups of the main window.

## User error protection
For some protection against user error, the program does not allow deleting elements of Mikrotik address lists, but only turning them on / off.
you can also add new items to the address list. They are added disabled.

## Third party copyright
This tool uses icons from [flaticon.com](https://www.flaticon.com/free-icons/google-plus). When distributing this content, you must comply with the requirements of the copyright holder.

## Denial of responsibility
Everything you use includes risks of occurrence and all possible consequences and potential losses that occur at your own risk. The author does not recommend using any parts of this code for any kind of responsible applications. Use only at your own risk.