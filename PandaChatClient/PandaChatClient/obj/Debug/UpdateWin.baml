   M S B A M L   `   `   `  ����J  FPandaChatClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:PandaChatClientPandaChatClient  5   	       PandaChatClient.UpdateWin   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:PandaChatClient     5   	   -   5          � Title$  	UpdateWin��5   	   $	��295��6   $��613.667��6(    � 
ResizeMode$ NoResize=�68    � WindowStartupLocation$ CenterScreen=�6N    � WindowStyle$
 None=�6s   .��5	      � ���	#FF1F1F1F.+�5
   
   �� -   5         ��Name$ label��5
      $+��%Доступно обновление��6   $
��Left=�6=   $��	10,10,0,0q�6X   $	��Top=�6k   
�������$��PTSansCaption)�6�   $
��Bold"�6�   $��18(�6�   �� 5   
   -   5        $ label2��5      $"��Текущая версия:��6    $
��Left=�6:   $��	39,95,0,0q�6U   $	��Top=�6h   
�������$��PTSansCaption)�6�   $
��Bold"�6�   �� 5   
   -   5        $ label3��5      $&�� Серверная версия:��6    $
��Left=�6<   $��
39,125,0,0q�6W   $	��Top=�6k   
�������$��PTSansCaption)�6�   $
��Bold"�6�   �� 5   
   -   5        $ CurrentVersionLabel��5      $	��0.0��6-   $
��Left=�6;   $��
160,95,0,0q�6V   $	��Top=�6j   $��PTSansCaption)�6�   
�������$
��Bold"�6�    J� RenderTransformOrigin$ 	0.462,0.5&�6�   �� 5   
   -   5        $ ServerVersionLabel��5      $	��0.1��6,   $
��Left=�6:   $��160,125,0,0q�6U   $	��Top=�6j   $��PTSansCaption)�6�   
�������$
��Bold"�6�   �� 5   
   -   5         ��Name$ ChangeUpdate��5      $
��Left=�6(   $	��242��6C   $��
222,10,0,0q�6P    �� TextWrapping$
 Wrap=�6d   $	��Top=�6x   $	��382��6�   ���	#FF9C9B9B���	#FF1F1F1F
�������$��PTSansCaption)�6�   $
��Bold"�6�   � 5   
   -   5         �Name$ ProgressDownload��5      $
��Left=�60   $��21��6K   $��
10,257,0,0q�6W   $	��Top=�6k   $	��594��6�   ���	#FF1F1F1F
���  ��6�   �� 5   
   -	   5        	 ��Name$	 ContinueUpdate��5      $��Продолжить��6)   $
��Left=�6>   $��
54,177,0,0q�6Y   $	��Top=�6m   $	��124��6�   $��21��6�   ���	#FF2F895A
�������$��PTSansCaption)�6�   $
��Bold"�6�   {�.  
 � Cursor$
 Arrow��6  6  �� 5   
   -
   5        $ label5��5      $-��'Состояние обновления��6    $
��Left=�6?   $��
10,231,0,0q�6Z   $	��Top=�6n   $��PTSansCaption)�6�   $
��Bold"�6�   
�������$��10(�6�   $��21��6�   $	��129��6�   5      5      