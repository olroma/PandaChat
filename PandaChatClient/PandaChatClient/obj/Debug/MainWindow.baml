   M S B A M L   `   `   `  ����J  FPandaChatClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:PandaChatClientPandaChatClient  5   	        PandaChatClient.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:PandaChatClient     5   	   -   5        &   System.Windows.Media.TextOptions    TextFormattingMode$  Display=�5   	    � UseLayoutRounding .  � Title$ 
MainWindow��5
   	   $	��548��6   $
��1016��6)    � WindowStyle$
 None=�6d   .��5      � $��0,0,0,-1q�6   ���	#FF1D1D1D.+�5   
   �� -   5         ��Name$ LogText��5   (    �� 
IsReadOnly . $��	10,41,0,0q�69   ���	#FF1D1D1D
�������$��PTSansCaption)�6v   $��14(�6�   $
��Bold"�6�   $
��Left=�6�   $	��837��6�   $	��430��6�   $	��Top=�6�   �� 5   
   -   5         ��Name$ UserListOnline��5      $	��430��6*   $��
852,41,0,0q�67   $	��Top=�6K   ���	#FF1D1D1D
�������$��PTSansCaption)�6�   $
��Bold"�6�   $
��Left=�6�   $	��142��6�   �� 5   
   -   5         ��Name$ SendMessageText��5      $��25��6+   $��
10,476,0,0q�67    �� TextWrapping$
 Wrap=�6K   $�� ��6_   $	��Top=�6g   ���	#FF1D1D1D
�������$��PTSansCaption)�6�   $
��Bold"�6�   $��14(�6�   $
��Left=�6  $	��837��6!  �� 5   
   -   5        	 ��Name$	 SendMessageButton��5      $��Отправить��6,   $��852,476,0,0q�6@   ���	#FF2F895A$��PTSansCaption)�6�   $
��Bold"�6�   
�������$
��Left=�6�   $	��142��6�   $��25��6�   $	��Top=�6�   �� 5   
   -   5        
 ��Name$
 MinimasedForm��5      $��_��6'   $
��Left=�63   $��	947,0,0,0q�6N   $	��Top=�6a   
�������$��PTSansCaption)�6  �� 5   
   -   5        $
 ExitFormLabel��5      $��X��6'   $
��Left=�63   $��	961,0,0,0q�6N   $	��Top=�6a   
�������$��26��6�   $��PTSansCaption)�6	  �� 5   
   -   5        $	 SendFileButton��5      $!��Отправить файл��6)   $
��Left=�6B   $��852,506,0,0q�6]   $	��Top=�6r   $	��142��6�   $��PTSansCaption)�6�   $
��Bold"�6�   ���	#FF2F895A
�������$��22��6
  �� 5   
   -	   5        $
 label��5      $'��!Добро пожаловать, ��6   $
��Left=�6<   $��10,4,0,0q�6W   $	��Top=�6i   
�������$��PTSansCaption)�6�   $��16(�6�   $
��Bold"�6�   �� 5   
   -
   5        $
 NameUserLabel��5      $	��NaN��6'   $
��Left=�65   $��	179,4,0,0q�6P   $	��Top=�6c   
�������$��PTSansCaption)�6�   $��16(�6�   $
��Bold"�6�   5      5      