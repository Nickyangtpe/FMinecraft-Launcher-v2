   M S B A M L   `   `   `  ����[  WFMinecraft Launcher v2 Installer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS.clr-namespace:FMinecraft_Launcher_v2_Installer FMinecraft_Launcher_v2_Installer  5   	   1    +FMinecraft_Launcher_v2_Installer.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   @local.clr-namespace:FMinecraft_Launcher_v2_Installer     5   	   -   5          � Title$#  FMinecraft Launcher Installer��5   	   $��367.6��6/   $	��604��6>    � WindowStartupLocation$ CenterScreen=�5	   	    � WindowStyle$
 None=�5
   	    � AllowsTransparency . 
������  � 	Resources 5         
FlatButton �� 
TargetType �� Property �� Value �� 
TargetType	 �� CornerRadius
 �� Triggers P� Property P� Value��%�  &	        �� 5   
   " ��6#   .�5      ��  � �� �#2E7D326+   �� 5       � ��
 �����6+   �� 5       � ��$ 0q�60   �� 5       � ��$
 15,5q�6(   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   $	 5��6K   ._�5      �� $��Center=�6/   $��Center=�6L   5      5      5      
 5      P� 5       �@J�IsMouseOver . . �5      ��  � �� �#1B5E2063   5      5!      5"      .��5$      �� $	 10��6   ���#1E1E1E._�5%   
   �  � RowDefinitions 5&      �� 5'      $��30 �6    �� 5(      $��* �6    �� 5)      $
��Auto �6    .+�5-      � -   5        ���#2E7D32.+�5.      �� $#��FMinecraft Launcher Installer��6   
�������$
��Bold"�6T   $��10,0,0,0q�6f   $��Center=�6x   �� 5/      -   5        $��X��5/      $��Right=�6%   $��30��6A   
������ $��0q�6e   
�������6�   50      � 53      $��1��6   $��20q�6     � ColumnDefinitions 54      �� 55      $��1* �6'   �� 56      $��2* �6'   .+�5:      �� $��0��6   $��0,0,20,0q�6-   .+�5;      �� $��Installation Steps��6    ���#E0E0E0$
��Bold"�6O   $��18(�6a   $��0,0,0,10q�6o   �� 5<      $��
1. Welcome��6    ���	#FFD1D1D1$	��0,5q�6I   �� 5=      $��2. License Agreement��6    ���#B0B0B0$	��0,5q�6Q   �� 5>      $��3. Installing��6    ���#B0B0B0$	��0,5q�6J   �� 5?      $��	4. Finish��6    ���#B0B0B0$	��0,5q�6F   5@      � 5C      $��1��6    5D      �� 5E      $
��Auto �6(   �� 5F      $��* �6(   �� 5G      $
��Auto �6(   .+�5J      �� $$��Welcome to FMinecraft Launcher��6    ���#E0E0E0$
��Bold"�6[   $��23(�6m   $��0,0,0,20q�6{   �� 5L      $��1��6    $
��Wrap=�6-   $��Center=�6A   ���#B0B0B0$	��114��6q   $��0,0.4,0,68.8q�6~   ��Thank you for choosing FMinecraft Launcher. This installer will guide you through the installation process. Click 'Next' to continue.5N      5O      5P      �� 5S      $��2��6    �� Orientation$ 
Horizontal=�6&   $��Right=�6?   $��20q�6[   .+�5T      �� -   5        $
��Back��5T      #��[  6(   $��0,0,10,-0.2q�6L   {�.  $	��120��6s   6   �� 5U      -   5        $
��Next��5U      #��[  6(   $��
0,0,0,-0.2q�6L   $	��120��6`   $��25��6l   $	��Top=�6x    �� 	IsDefault . 6�   5V      5W      5X      5Y      