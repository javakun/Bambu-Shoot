   M S B A M L   `   `   `  ����P  LBambuShootProject.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:BambuShootProject.WPFBambuShootProject.WPF  5   	   .    (BambuShootProject.WPF.ImageProcessWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   5local#clr-namespace:BambuShootProject.WPF     5   	     � Title$  ImageProcessWindow��5   
   $	��720��6%   $
��1280��62   .��5	      � .+�5
   
   �� -   5         ��Name$ image��5
      $
��Left=�6   $	��218��6:   $��
785,64,0,0q�6G   $	��Top=�6[   $	��313��6s   �� 5   
   -   5         ��Name$ textBox��5      $
��Left=�6#   $��30��6>   $��145,407,0,0q�6J    �� TextWrapping$
 Wrap=�6_   $	��165��6s   $	��Top=�6~   $	��203��6�   $��18(�6�   �� 5   
   -   5         ��Name$ 	textBlock��5      $
��Left=�6'   $��145,372,0,0q�6B   $
��Wrap=�6W   $��Threshold (0-255)��6k   $	��Top=�6�   $��27��6�   $	��231��6�   $��18(�6�   $
��Bold"�6�   �� 5   
   -   5         ��Name$ button��5      $��Preview��6!   $
��Left=�63   $��572,285,0,0q�6N   $	��Top=�6c   $	��115��6{   $��46��6�   $��16(�6�   $
��Bold"�6�   �� 5   
   -   5        $ image1��5      $
��Left=�6    $	��218��6;   $��785,391,0,0q�6H   $	��Top=�6]   $	��313��6u   �� 5   
   -   5        $ 
textBlock1��5      $
��Left=�6(   $��
807,43,0,0q�6C   $
��Wrap=�6W   $��Original Image��6k   $	��Top=�6�   �� 5   
   -   5        $ 
textBlock2��5      $
��Left=�6(   $��807,370,0,0q�6C   $
��Wrap=�6X   $��Preview Image��6l   $	��Top=�6�   	� 5   
   -   5         	�Name$ radioButton_TH��5      $��Black and White Threshold��6.    	� 	GroupName$ method��6R   $
��Left=�6e    d� 	IsChecked$
 TrueY�6�   $��145,171,0,0q�6�   $	��Top=�6�   $��27��6�   $	��231��6�   $��18(�6�   6�   	� 5   
   -	   5        $ radioButton_ATH��5      $"��Grayscale Adaptive Threshold��6/   $ method��6V   $
��Left=�6i   $��145,203,0,0q�6�   $	��Top=�6�   $��27��6�   $	��265��6�   $��18(�6�   6�   �� 5   
   -
   5        $ 
textBlock3��5      $
��Left=�6(   $��145,140,0,0q�6C   $
��Wrap=�6X   $��Method ��6l   $	��Top=�6{   $��26��6�   $	��113��6�   $��18(�6�   $
��Bold"�6�   �� 5   
   -   5        $ process��5      $��Process��6"   $
��Left=�64   $��542,559,0,0q�6O   $	��Top=�6d   $	��173��6|   $��50��6�   $��20(�6�   $
��Bold"�6�   6�   �� 5   
   -   5        $ 
imageTitle��5      $
��Left=�6(   $��1175,116,-373,0q�6C   $
��Wrap=�6\   $y�Hidden=�6p   $	��Top=�6�   $��59��6�   �� 6�   $�	TextBlock��6�   6�   �� 5   
   -   5        $ dateOfHarvest��5      $
��Left=�6+   $��1175,116,-373,0q�6F   $
��Wrap=�6_   $��	TextBlock��6s   $	��Top=�6�   $��59��6�   $y�Hidden=�6�   �� 5   
   -   5        $ nameOfSpecies��5      $
��Left=�6+   $��1175,116,-373,0q�6F   $
��Wrap=�6_   $��	TextBlock��6s   $	��Top=�6�   $��59��6�   $y�Hidden=�6�   �� 5   
   -   5        $ location��5      $
��Left=�6&   $��1175,116,-373,0q�6A   $
��Wrap=�6Z   $��	TextBlock��6n   $	��Top=�6   $��59��6�   $y�Hidden=�6�   5      5      