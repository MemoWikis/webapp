﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuNo.Master" Inherits="System.Web.Mvc.ViewPage<WelcomeModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    
    <link href="/Style/site.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .fa-hover a {
            line-height: 32px;
            color: black;
        }

    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
<div class="container">
  
  <div class="row fontawesome-icon-list">
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="glass|martini|drink|bar|alcohol|liquor">
      <a href="../icon/glass"><i class="fa fa-glass"></i> glass</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="music|note|sound">
      <a href="../icon/music"><i class="fa fa-music"></i> music</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="search|magnify|zoom|enlarge|bigger">
      <a href="../icon/search"><i class="fa fa-search"></i> search</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="envelope-o|email|support|e-mail|letter|mail|notification">
      <a href="../icon/envelope-o"><i class="fa fa-envelope-o"></i> envelope-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="heart|love|like|favorite">
      <a href="../icon/heart"><i class="fa fa-heart"></i> heart</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="star|award|achievement|night|rating|score">
      <a href="../icon/star"><i class="fa fa-star"></i> star</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="star-o|award|achievement|night|rating|score">
      <a href="../icon/star-o"><i class="fa fa-star-o"></i> star-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="user|person|man|head|profile">
      <a href="../icon/user"><i class="fa fa-user"></i> user</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="film|movie">
      <a href="../icon/film"><i class="fa fa-film"></i> film</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="th-large|blocks|squares|boxes">
      <a href="../icon/th-large"><i class="fa fa-th-large"></i> th-large</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="th|blocks|squares|boxes">
      <a href="../icon/th"><i class="fa fa-th"></i> th</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="th-list|ul|ol|checklist|finished|completed|done|todo">
      <a href="../icon/th-list"><i class="fa fa-th-list"></i> th-list</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="check|checkmark|done|todo|agree|accept|confirm">
      <a href="../icon/check"><i class="fa fa-check"></i> check</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="times|remove|close|close|exit|x">
      <a href="../icon/times"><i class="fa fa-times"></i> times</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="search-plus|magnify|zoom|enlarge|bigger">
      <a href="../icon/search-plus"><i class="fa fa-search-plus"></i> search-plus</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="search-minus|magnify|minify|zoom|smaller">
      <a href="../icon/search-minus"><i class="fa fa-search-minus"></i> search-minus</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="power-off|on">
      <a href="../icon/power-off"><i class="fa fa-power-off"></i> power-off</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="signal">
      <a href="../icon/signal"><i class="fa fa-signal"></i> signal</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="cog|gear|settings">
      <a href="../icon/cog"><i class="fa fa-cog"></i> cog</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="trash-o|garbage|delete|remove|trash|hide">
      <a href="../icon/trash-o"><i class="fa fa-trash-o"></i> trash-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="home|main|house">
      <a href="../icon/home"><i class="fa fa-home"></i> home</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="file-o|new|page|pdf|document">
      <a href="../icon/file-o"><i class="fa fa-file-o"></i> file-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="clock-o|watch|timer|late|timestamp">
      <a href="../icon/clock-o"><i class="fa fa-clock-o"></i> clock-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="road|street">
      <a href="../icon/road"><i class="fa fa-road"></i> road</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="download|import">
      <a href="../icon/download"><i class="fa fa-download"></i> download</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="arrow-circle-o-down|download">
      <a href="../icon/arrow-circle-o-down"><i class="fa fa-arrow-circle-o-down"></i> arrow-circle-o-down</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="arrow-circle-o-up">
      <a href="../icon/arrow-circle-o-up"><i class="fa fa-arrow-circle-o-up"></i> arrow-circle-o-up</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="inbox">
      <a href="../icon/inbox"><i class="fa fa-inbox"></i> inbox</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="play-circle-o">
      <a href="../icon/play-circle-o"><i class="fa fa-play-circle-o"></i> play-circle-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="repeat|rotate-right|redo|forward">
      <a href="../icon/repeat"><i class="fa fa-repeat"></i> repeat</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="refresh|reload">
      <a href="../icon/refresh"><i class="fa fa-refresh"></i> refresh</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="list-alt|ul|ol|checklist|finished|completed|done|todo">
      <a href="../icon/list-alt"><i class="fa fa-list-alt"></i> list-alt</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="lock|protect|admin">
      <a href="../icon/lock"><i class="fa fa-lock"></i> lock</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="flag|report|notification|notify">
      <a href="../icon/flag"><i class="fa fa-flag"></i> flag</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="headphones|sound|listen|music">
      <a href="../icon/headphones"><i class="fa fa-headphones"></i> headphones</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="volume-off|mute|sound|music">
      <a href="../icon/volume-off"><i class="fa fa-volume-off"></i> volume-off</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="volume-down|lower|quieter|sound|music">
      <a href="../icon/volume-down"><i class="fa fa-volume-down"></i> volume-down</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="volume-up|higher|louder|sound|music">
      <a href="../icon/volume-up"><i class="fa fa-volume-up"></i> volume-up</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="qrcode|scan">
      <a href="../icon/qrcode"><i class="fa fa-qrcode"></i> qrcode</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="barcode|scan">
      <a href="../icon/barcode"><i class="fa fa-barcode"></i> barcode</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="tag|label">
      <a href="../icon/tag"><i class="fa fa-tag"></i> tag</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="tags|labels">
      <a href="../icon/tags"><i class="fa fa-tags"></i> tags</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="book|read|documentation">
      <a href="../icon/book"><i class="fa fa-book"></i> book</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="bookmark|save">
      <a href="../icon/bookmark"><i class="fa fa-bookmark"></i> bookmark</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="print">
      <a href="../icon/print"><i class="fa fa-print"></i> print</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="camera|photo|picture|record">
      <a href="../icon/camera"><i class="fa fa-camera"></i> camera</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="font|text">
      <a href="../icon/font"><i class="fa fa-font"></i> font</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="bold">
      <a href="../icon/bold"><i class="fa fa-bold"></i> bold</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="italic|italics">
      <a href="../icon/italic"><i class="fa fa-italic"></i> italic</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="text-height">
      <a href="../icon/text-height"><i class="fa fa-text-height"></i> text-height</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="text-width">
      <a href="../icon/text-width"><i class="fa fa-text-width"></i> text-width</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="align-left|text">
      <a href="../icon/align-left"><i class="fa fa-align-left"></i> align-left</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="align-center|middle|text">
      <a href="../icon/align-center"><i class="fa fa-align-center"></i> align-center</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="align-right|text">
      <a href="../icon/align-right"><i class="fa fa-align-right"></i> align-right</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="align-justify|text">
      <a href="../icon/align-justify"><i class="fa fa-align-justify"></i> align-justify</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="list|ul|ol|checklist|finished|completed|done|todo">
      <a href="../icon/list"><i class="fa fa-list"></i> list</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="outdent|dedent">
      <a href="../icon/outdent"><i class="fa fa-outdent"></i> outdent</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="indent">
      <a href="../icon/indent"><i class="fa fa-indent"></i> indent</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="video-camera|film|movie|record">
      <a href="../icon/video-camera"><i class="fa fa-video-camera"></i> video-camera</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="picture-o|photo|image">
      <a href="../icon/picture-o"><i class="fa fa-picture-o"></i> picture-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="pencil|write|edit|update">
      <a href="../icon/pencil"><i class="fa fa-pencil"></i> pencil</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="map-marker|map|pin|location|coordinates|localize|address|travel|where|place">
      <a href="../icon/map-marker"><i class="fa fa-map-marker"></i> map-marker</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="adjust|contrast">
      <a href="../icon/adjust"><i class="fa fa-adjust"></i> adjust</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="tint|raindrop">
      <a href="../icon/tint"><i class="fa fa-tint"></i> tint</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="pencil-square-o|edit|write|edit|update">
      <a href="../icon/pencil-square-o"><i class="fa fa-pencil-square-o"></i> pencil-square-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="share-square-o|social|send">
      <a href="../icon/share-square-o"><i class="fa fa-share-square-o"></i> share-square-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="check-square-o|todo|done|agree|accept|confirm">
      <a href="../icon/check-square-o"><i class="fa fa-check-square-o"></i> check-square-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="arrows|move|reorder|resize">
      <a href="../icon/arrows"><i class="fa fa-arrows"></i> arrows</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="step-backward|rewind|previous|beginning|start|first">
      <a href="../icon/step-backward"><i class="fa fa-step-backward"></i> step-backward</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="fast-backward|rewind|previous|beginning|start|first">
      <a href="../icon/fast-backward"><i class="fa fa-fast-backward"></i> fast-backward</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="backward|rewind|previous">
      <a href="../icon/backward"><i class="fa fa-backward"></i> backward</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="play|start|playing|music|sound">
      <a href="../icon/play"><i class="fa fa-play"></i> play</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="pause|wait">
      <a href="../icon/pause"><i class="fa fa-pause"></i> pause</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="stop|block|box|square">
      <a href="../icon/stop"><i class="fa fa-stop"></i> stop</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="forward|forward|next">
      <a href="../icon/forward"><i class="fa fa-forward"></i> forward</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="fast-forward|next|end|last">
      <a href="../icon/fast-forward"><i class="fa fa-fast-forward"></i> fast-forward</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="step-forward|next|end|last">
      <a href="../icon/step-forward"><i class="fa fa-step-forward"></i> step-forward</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="eject">
      <a href="../icon/eject"><i class="fa fa-eject"></i> eject</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="chevron-left|bracket|previous|back">
      <a href="../icon/chevron-left"><i class="fa fa-chevron-left"></i> chevron-left</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="chevron-right|bracket|next|forward">
      <a href="../icon/chevron-right"><i class="fa fa-chevron-right"></i> chevron-right</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="plus-circle|add|new|create|expand">
      <a href="../icon/plus-circle"><i class="fa fa-plus-circle"></i> plus-circle</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="minus-circle|delete|remove|trash|hide">
      <a href="../icon/minus-circle"><i class="fa fa-minus-circle"></i> minus-circle</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="times-circle|close|exit|x">
      <a href="../icon/times-circle"><i class="fa fa-times-circle"></i> times-circle</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="check-circle|todo|done|agree|accept|confirm">
      <a href="../icon/check-circle"><i class="fa fa-check-circle"></i> check-circle</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="question-circle|help|information|unknown|support">
      <a href="../icon/question-circle"><i class="fa fa-question-circle"></i> question-circle</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="info-circle|help|information|more|details">
      <a href="../icon/info-circle"><i class="fa fa-info-circle"></i> info-circle</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="crosshairs|picker">
      <a href="../icon/crosshairs"><i class="fa fa-crosshairs"></i> crosshairs</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="times-circle-o|close|exit|x">
      <a href="../icon/times-circle-o"><i class="fa fa-times-circle-o"></i> times-circle-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="check-circle-o|todo|done|agree|accept|confirm">
      <a href="../icon/check-circle-o"><i class="fa fa-check-circle-o"></i> check-circle-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="ban|delete|remove|trash|hide|block|stop|abort|cancel">
      <a href="../icon/ban"><i class="fa fa-ban"></i> ban</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="arrow-left|previous|back">
      <a href="../icon/arrow-left"><i class="fa fa-arrow-left"></i> arrow-left</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="arrow-right|next|forward">
      <a href="../icon/arrow-right"><i class="fa fa-arrow-right"></i> arrow-right</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="arrow-up">
      <a href="../icon/arrow-up"><i class="fa fa-arrow-up"></i> arrow-up</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="arrow-down|download">
      <a href="../icon/arrow-down"><i class="fa fa-arrow-down"></i> arrow-down</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="share|mail-forward">
      <a href="../icon/share"><i class="fa fa-share"></i> share</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="expand|enlarge|bigger|resize">
      <a href="../icon/expand"><i class="fa fa-expand"></i> expand</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="compress|collapse|combine|contract|merge|smaller">
      <a href="../icon/compress"><i class="fa fa-compress"></i> compress</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="plus|add|new|create|expand">
      <a href="../icon/plus"><i class="fa fa-plus"></i> plus</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="minus|hide|minify|delete|remove|trash|hide|collapse">
      <a href="../icon/minus"><i class="fa fa-minus"></i> minus</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="asterisk|details">
      <a href="../icon/asterisk"><i class="fa fa-asterisk"></i> asterisk</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="exclamation-circle|warning|error|problem|notification|alert">
      <a href="../icon/exclamation-circle"><i class="fa fa-exclamation-circle"></i> exclamation-circle</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="gift|present">
      <a href="../icon/gift"><i class="fa fa-gift"></i> gift</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="leaf|eco|nature">
      <a href="../icon/leaf"><i class="fa fa-leaf"></i> leaf</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="fire|flame|hot|popular">
      <a href="../icon/fire"><i class="fa fa-fire"></i> fire</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="eye|show|visible|views">
      <a href="../icon/eye"><i class="fa fa-eye"></i> eye</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="eye-slash|toggle|show|hide|visible|visiblity|views">
      <a href="../icon/eye-slash"><i class="fa fa-eye-slash"></i> eye-slash</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="exclamation-triangle|warning|warning|error|problem|notification|alert">
      <a href="../icon/exclamation-triangle"><i class="fa fa-exclamation-triangle"></i> exclamation-triangle</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="plane|travel|trip|location|destination|airplane|fly|mode">
      <a href="../icon/plane"><i class="fa fa-plane"></i> plane</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="calendar|date|time|when">
      <a href="../icon/calendar"><i class="fa fa-calendar"></i> calendar</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="random|sort|shuffle">
      <a href="../icon/random"><i class="fa fa-random"></i> random</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="comment|speech|notification|note|chat|bubble|feedback">
      <a href="../icon/comment"><i class="fa fa-comment"></i> comment</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="magnet">
      <a href="../icon/magnet"><i class="fa fa-magnet"></i> magnet</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="chevron-up">
      <a href="../icon/chevron-up"><i class="fa fa-chevron-up"></i> chevron-up</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="chevron-down">
      <a href="../icon/chevron-down"><i class="fa fa-chevron-down"></i> chevron-down</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="retweet|refresh|reload|share">
      <a href="../icon/retweet"><i class="fa fa-retweet"></i> retweet</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="shopping-cart|checkout|buy|purchase|payment">
      <a href="../icon/shopping-cart"><i class="fa fa-shopping-cart"></i> shopping-cart</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="folder">
      <a href="../icon/folder"><i class="fa fa-folder"></i> folder</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="folder-open">
      <a href="../icon/folder-open"><i class="fa fa-folder-open"></i> folder-open</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="arrows-v|resize">
      <a href="../icon/arrows-v"><i class="fa fa-arrows-v"></i> arrows-v</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="arrows-h|resize">
      <a href="../icon/arrows-h"><i class="fa fa-arrows-h"></i> arrows-h</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="bar-chart|bar-chart-o|graph">
      <a href="../icon/bar-chart"><i class="fa fa-bar-chart"></i> bar-chart</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="twitter-square|tweet|social network">
      <a href="../icon/twitter-square"><i class="fa fa-twitter-square"></i> twitter-square</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="facebook-square|social network">
      <a href="../icon/facebook-square"><i class="fa fa-facebook-square"></i> facebook-square</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="camera-retro|photo|picture|record">
      <a href="../icon/camera-retro"><i class="fa fa-camera-retro"></i> camera-retro</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="key|unlock|password">
      <a href="../icon/key"><i class="fa fa-key"></i> key</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="cogs|gears|settings">
      <a href="../icon/cogs"><i class="fa fa-cogs"></i> cogs</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="comments|conversation|notification|notes">
      <a href="../icon/comments"><i class="fa fa-comments"></i> comments</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="thumbs-o-up|like|approve|favorite|agree|hand">
      <a href="../icon/thumbs-o-up"><i class="fa fa-thumbs-o-up"></i> thumbs-o-up</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="thumbs-o-down|dislike|disapprove|disagree|hand">
      <a href="../icon/thumbs-o-down"><i class="fa fa-thumbs-o-down"></i> thumbs-o-down</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="star-half|award|achievement|rating|score">
      <a href="../icon/star-half"><i class="fa fa-star-half"></i> star-half</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="heart-o|love|like|favorite">
      <a href="../icon/heart-o"><i class="fa fa-heart-o"></i> heart-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="sign-out|log out|logout|leave|exit|arrow">
      <a href="../icon/sign-out"><i class="fa fa-sign-out"></i> sign-out</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="linkedin-square">
      <a href="../icon/linkedin-square"><i class="fa fa-linkedin-square"></i> linkedin-square</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="thumb-tack|marker|pin|location|coordinates">
      <a href="../icon/thumb-tack"><i class="fa fa-thumb-tack"></i> thumb-tack</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="external-link|open|new">
      <a href="../icon/external-link"><i class="fa fa-external-link"></i> external-link</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="sign-in|enter|join|log in|login|sign up|sign in|signin|signup|arrow">
      <a href="../icon/sign-in"><i class="fa fa-sign-in"></i> sign-in</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="trophy|award|achievement|winner|game">
      <a href="../icon/trophy"><i class="fa fa-trophy"></i> trophy</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="github-square|octocat">
      <a href="../icon/github-square"><i class="fa fa-github-square"></i> github-square</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="upload|import">
      <a href="../icon/upload"><i class="fa fa-upload"></i> upload</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="lemon-o">
      <a href="../icon/lemon-o"><i class="fa fa-lemon-o"></i> lemon-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="phone|call|voice|number|support|earphone">
      <a href="../icon/phone"><i class="fa fa-phone"></i> phone</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="square-o|block|square|box">
      <a href="../icon/square-o"><i class="fa fa-square-o"></i> square-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="bookmark-o|save">
      <a href="../icon/bookmark-o"><i class="fa fa-bookmark-o"></i> bookmark-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="phone-square|call|voice|number|support">
      <a href="../icon/phone-square"><i class="fa fa-phone-square"></i> phone-square</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="twitter|tweet|social network">
      <a href="../icon/twitter"><i class="fa fa-twitter"></i> twitter</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="facebook|facebook-f|social network">
      <a href="../icon/facebook"><i class="fa fa-facebook"></i> facebook</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="github|octocat">
      <a href="../icon/github"><i class="fa fa-github"></i> github</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="unlock|protect|admin|password">
      <a href="../icon/unlock"><i class="fa fa-unlock"></i> unlock</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="credit-card|money|buy|debit|checkout|purchase|payment">
      <a href="../icon/credit-card"><i class="fa fa-credit-card"></i> credit-card</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="rss|feed|blog">
      <a href="../icon/rss"><i class="fa fa-rss"></i> rss</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="hdd-o|harddrive|hard drive|storage|save">
      <a href="../icon/hdd-o"><i class="fa fa-hdd-o"></i> hdd-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="bullhorn|announcement|share|broadcast|louder">
      <a href="../icon/bullhorn"><i class="fa fa-bullhorn"></i> bullhorn</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="bell|alert|reminder|notification">
      <a href="../icon/bell"><i class="fa fa-bell"></i> bell</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="certificate|badge|star">
      <a href="../icon/certificate"><i class="fa fa-certificate"></i> certificate</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="hand-o-right|point|right|next|forward">
      <a href="../icon/hand-o-right"><i class="fa fa-hand-o-right"></i> hand-o-right</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="hand-o-left|point|left|previous|back">
      <a href="../icon/hand-o-left"><i class="fa fa-hand-o-left"></i> hand-o-left</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="hand-o-up|point">
      <a href="../icon/hand-o-up"><i class="fa fa-hand-o-up"></i> hand-o-up</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="hand-o-down|point">
      <a href="../icon/hand-o-down"><i class="fa fa-hand-o-down"></i> hand-o-down</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="arrow-circle-left|previous|back">
      <a href="../icon/arrow-circle-left"><i class="fa fa-arrow-circle-left"></i> arrow-circle-left</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="arrow-circle-right|next|forward">
      <a href="../icon/arrow-circle-right"><i class="fa fa-arrow-circle-right"></i> arrow-circle-right</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="arrow-circle-up">
      <a href="../icon/arrow-circle-up"><i class="fa fa-arrow-circle-up"></i> arrow-circle-up</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="arrow-circle-down|download">
      <a href="../icon/arrow-circle-down"><i class="fa fa-arrow-circle-down"></i> arrow-circle-down</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="globe|world|planet|map|place|travel|earth|global|translate|all|language|localize|location|coordinates|country">
      <a href="../icon/globe"><i class="fa fa-globe"></i> globe</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="wrench|settings|fix|update">
      <a href="../icon/wrench"><i class="fa fa-wrench"></i> wrench</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="tasks|progress|loading|downloading|downloads|settings">
      <a href="../icon/tasks"><i class="fa fa-tasks"></i> tasks</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="filter|funnel|options">
      <a href="../icon/filter"><i class="fa fa-filter"></i> filter</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="briefcase|work|business|office|luggage|bag">
      <a href="../icon/briefcase"><i class="fa fa-briefcase"></i> briefcase</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="arrows-alt|expand|enlarge|fullscreen|bigger|move|reorder|resize">
      <a href="../icon/arrows-alt"><i class="fa fa-arrows-alt"></i> arrows-alt</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="users|group|people|profiles|persons">
      <a href="../icon/users"><i class="fa fa-users"></i> users</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="link|chain|chain">
      <a href="../icon/link"><i class="fa fa-link"></i> link</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="cloud|save">
      <a href="../icon/cloud"><i class="fa fa-cloud"></i> cloud</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="flask|science|beaker|experimental|labs">
      <a href="../icon/flask"><i class="fa fa-flask"></i> flask</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="scissors|cut">
      <a href="../icon/scissors"><i class="fa fa-scissors"></i> scissors</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="files-o|copy|duplicate">
      <a href="../icon/files-o"><i class="fa fa-files-o"></i> files-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="paperclip|attachment">
      <a href="../icon/paperclip"><i class="fa fa-paperclip"></i> paperclip</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="floppy-o|save">
      <a href="../icon/floppy-o"><i class="fa fa-floppy-o"></i> floppy-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="square|block|box">
      <a href="../icon/square"><i class="fa fa-square"></i> square</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="bars|navicon|reorder|menu|drag|reorder|settings|list|ul|ol|checklist|todo|list|hamburger">
      <a href="../icon/bars"><i class="fa fa-bars"></i> bars</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="list-ul|ul|ol|checklist|todo|list">
      <a href="../icon/list-ul"><i class="fa fa-list-ul"></i> list-ul</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="list-ol|ul|ol|checklist|list|todo|list|numbers">
      <a href="../icon/list-ol"><i class="fa fa-list-ol"></i> list-ol</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="strikethrough">
      <a href="../icon/strikethrough"><i class="fa fa-strikethrough"></i> strikethrough</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="underline">
      <a href="../icon/underline"><i class="fa fa-underline"></i> underline</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="table|data|excel|spreadsheet">
      <a href="../icon/table"><i class="fa fa-table"></i> table</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="magic|wizard|automatic|autocomplete">
      <a href="../icon/magic"><i class="fa fa-magic"></i> magic</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="truck|shipping">
      <a href="../icon/truck"><i class="fa fa-truck"></i> truck</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="pinterest">
      <a href="../icon/pinterest"><i class="fa fa-pinterest"></i> pinterest</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="pinterest-square">
      <a href="../icon/pinterest-square"><i class="fa fa-pinterest-square"></i> pinterest-square</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="google-plus-square|social network">
      <a href="../icon/google-plus-square"><i class="fa fa-google-plus-square"></i> google-plus-square</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="google-plus|social network">
      <a href="../icon/google-plus"><i class="fa fa-google-plus"></i> google-plus</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="money|cash|money|buy|checkout|purchase|payment">
      <a href="../icon/money"><i class="fa fa-money"></i> money</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="caret-down|more|dropdown|menu">
      <a href="../icon/caret-down"><i class="fa fa-caret-down"></i> caret-down</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="caret-up">
      <a href="../icon/caret-up"><i class="fa fa-caret-up"></i> caret-up</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="caret-left|previous|back">
      <a href="../icon/caret-left"><i class="fa fa-caret-left"></i> caret-left</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="caret-right|next|forward">
      <a href="../icon/caret-right"><i class="fa fa-caret-right"></i> caret-right</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="columns|split|panes">
      <a href="../icon/columns"><i class="fa fa-columns"></i> columns</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="sort|unsorted|order">
      <a href="../icon/sort"><i class="fa fa-sort"></i> sort</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="sort-desc|sort-down|dropdown|more|menu">
      <a href="../icon/sort-desc"><i class="fa fa-sort-desc"></i> sort-desc</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="sort-asc|sort-up">
      <a href="../icon/sort-asc"><i class="fa fa-sort-asc"></i> sort-asc</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="envelope|email|e-mail|letter|support|mail|notification">
      <a href="../icon/envelope"><i class="fa fa-envelope"></i> envelope</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="linkedin">
      <a href="../icon/linkedin"><i class="fa fa-linkedin"></i> linkedin</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="undo|rotate-left|back">
      <a href="../icon/undo"><i class="fa fa-undo"></i> undo</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="gavel|legal">
      <a href="../icon/gavel"><i class="fa fa-gavel"></i> gavel</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="tachometer|dashboard">
      <a href="../icon/tachometer"><i class="fa fa-tachometer"></i> tachometer</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="comment-o|notification|note">
      <a href="../icon/comment-o"><i class="fa fa-comment-o"></i> comment-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="comments-o|conversation|notification|notes">
      <a href="../icon/comments-o"><i class="fa fa-comments-o"></i> comments-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="bolt|flash|lightning|weather">
      <a href="../icon/bolt"><i class="fa fa-bolt"></i> bolt</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="sitemap|directory|hierarchy|organization">
      <a href="../icon/sitemap"><i class="fa fa-sitemap"></i> sitemap</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="umbrella">
      <a href="../icon/umbrella"><i class="fa fa-umbrella"></i> umbrella</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="clipboard|paste|copy">
      <a href="../icon/clipboard"><i class="fa fa-clipboard"></i> clipboard</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="lightbulb-o|idea|inspiration">
      <a href="../icon/lightbulb-o"><i class="fa fa-lightbulb-o"></i> lightbulb-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="exchange|transfer">
      <a href="../icon/exchange"><i class="fa fa-exchange"></i> exchange</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="cloud-download|import">
      <a href="../icon/cloud-download"><i class="fa fa-cloud-download"></i> cloud-download</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="cloud-upload|import">
      <a href="../icon/cloud-upload"><i class="fa fa-cloud-upload"></i> cloud-upload</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="user-md|doctor|profile|medical|nurse">
      <a href="../icon/user-md"><i class="fa fa-user-md"></i> user-md</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="stethoscope">
      <a href="../icon/stethoscope"><i class="fa fa-stethoscope"></i> stethoscope</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="suitcase|trip|luggage|travel|move|baggage">
      <a href="../icon/suitcase"><i class="fa fa-suitcase"></i> suitcase</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="bell-o|alert|reminder|notification">
      <a href="../icon/bell-o"><i class="fa fa-bell-o"></i> bell-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="coffee|morning|mug|breakfast|tea|drink|cafe">
      <a href="../icon/coffee"><i class="fa fa-coffee"></i> coffee</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="cutlery|food|restaurant|spoon|knife|dinner|eat">
      <a href="../icon/cutlery"><i class="fa fa-cutlery"></i> cutlery</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="file-text-o|new|page|pdf|document">
      <a href="../icon/file-text-o"><i class="fa fa-file-text-o"></i> file-text-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="building-o|work|business|apartment|office|company">
      <a href="../icon/building-o"><i class="fa fa-building-o"></i> building-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="hospital-o|building">
      <a href="../icon/hospital-o"><i class="fa fa-hospital-o"></i> hospital-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="ambulance|support|help">
      <a href="../icon/ambulance"><i class="fa fa-ambulance"></i> ambulance</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="medkit|first aid|firstaid|help|support|health">
      <a href="../icon/medkit"><i class="fa fa-medkit"></i> medkit</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="fighter-jet|fly|plane|airplane|quick|fast|travel">
      <a href="../icon/fighter-jet"><i class="fa fa-fighter-jet"></i> fighter-jet</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="beer|alcohol|stein|drink|mug|bar|liquor">
      <a href="../icon/beer"><i class="fa fa-beer"></i> beer</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="h-square|hospital|hotel">
      <a href="../icon/h-square"><i class="fa fa-h-square"></i> h-square</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="plus-square|add|new|create|expand">
      <a href="../icon/plus-square"><i class="fa fa-plus-square"></i> plus-square</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="angle-double-left|laquo|quote|previous|back">
      <a href="../icon/angle-double-left"><i class="fa fa-angle-double-left"></i> angle-double-left</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="angle-double-right|raquo|quote|next|forward">
      <a href="../icon/angle-double-right"><i class="fa fa-angle-double-right"></i> angle-double-right</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="angle-double-up">
      <a href="../icon/angle-double-up"><i class="fa fa-angle-double-up"></i> angle-double-up</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="angle-double-down">
      <a href="../icon/angle-double-down"><i class="fa fa-angle-double-down"></i> angle-double-down</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="angle-left|previous|back">
      <a href="../icon/angle-left"><i class="fa fa-angle-left"></i> angle-left</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="angle-right|next|forward">
      <a href="../icon/angle-right"><i class="fa fa-angle-right"></i> angle-right</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="angle-up">
      <a href="../icon/angle-up"><i class="fa fa-angle-up"></i> angle-up</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="angle-down">
      <a href="../icon/angle-down"><i class="fa fa-angle-down"></i> angle-down</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="desktop|monitor|screen|desktop|computer|demo|device">
      <a href="../icon/desktop"><i class="fa fa-desktop"></i> desktop</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="laptop|demo|computer|device">
      <a href="../icon/laptop"><i class="fa fa-laptop"></i> laptop</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="tablet|ipad|device">
      <a href="../icon/tablet"><i class="fa fa-tablet"></i> tablet</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="mobile|mobile-phone|cell phone|cellphone|text|call|iphone|number">
      <a href="../icon/mobile"><i class="fa fa-mobile"></i> mobile</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="circle-o">
      <a href="../icon/circle-o"><i class="fa fa-circle-o"></i> circle-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="quote-left">
      <a href="../icon/quote-left"><i class="fa fa-quote-left"></i> quote-left</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="quote-right">
      <a href="../icon/quote-right"><i class="fa fa-quote-right"></i> quote-right</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="spinner|loading|progress">
      <a href="../icon/spinner"><i class="fa fa-spinner"></i> spinner</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="circle|dot|notification">
      <a href="../icon/circle"><i class="fa fa-circle"></i> circle</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="reply|mail-reply">
      <a href="../icon/reply"><i class="fa fa-reply"></i> reply</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="github-alt|octocat">
      <a href="../icon/github-alt"><i class="fa fa-github-alt"></i> github-alt</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="folder-o">
      <a href="../icon/folder-o"><i class="fa fa-folder-o"></i> folder-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="folder-open-o">
      <a href="../icon/folder-open-o"><i class="fa fa-folder-open-o"></i> folder-open-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="smile-o|emoticon|happy|approve|satisfied|rating">
      <a href="../icon/smile-o"><i class="fa fa-smile-o"></i> smile-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="frown-o|emoticon|sad|disapprove|rating">
      <a href="../icon/frown-o"><i class="fa fa-frown-o"></i> frown-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="meh-o|emoticon|rating|neutral">
      <a href="../icon/meh-o"><i class="fa fa-meh-o"></i> meh-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="gamepad|controller">
      <a href="../icon/gamepad"><i class="fa fa-gamepad"></i> gamepad</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="keyboard-o|type|input">
      <a href="../icon/keyboard-o"><i class="fa fa-keyboard-o"></i> keyboard-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="flag-o|report|notification">
      <a href="../icon/flag-o"><i class="fa fa-flag-o"></i> flag-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="flag-checkered|report|notification|notify">
      <a href="../icon/flag-checkered"><i class="fa fa-flag-checkered"></i> flag-checkered</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="terminal|command|prompt|code">
      <a href="../icon/terminal"><i class="fa fa-terminal"></i> terminal</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="code|html|brackets">
      <a href="../icon/code"><i class="fa fa-code"></i> code</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="reply-all|mail-reply-all">
      <a href="../icon/reply-all"><i class="fa fa-reply-all"></i> reply-all</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="star-half-o|star-half-empty|star-half-full|award|achievement|rating|score">
      <a href="../icon/star-half-o"><i class="fa fa-star-half-o"></i> star-half-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="location-arrow|map|coordinates|location|address|place|where">
      <a href="../icon/location-arrow"><i class="fa fa-location-arrow"></i> location-arrow</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="crop">
      <a href="../icon/crop"><i class="fa fa-crop"></i> crop</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="code-fork|git|fork|vcs|svn|github|rebase|version|merge">
      <a href="../icon/code-fork"><i class="fa fa-code-fork"></i> code-fork</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="chain-broken|unlink|remove">
      <a href="../icon/chain-broken"><i class="fa fa-chain-broken"></i> chain-broken</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="question|help|information|unknown|support">
      <a href="../icon/question"><i class="fa fa-question"></i> question</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="info|help|information|more|details">
      <a href="../icon/info"><i class="fa fa-info"></i> info</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="exclamation|warning|error|problem|notification|notify|alert">
      <a href="../icon/exclamation"><i class="fa fa-exclamation"></i> exclamation</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="superscript|exponential">
      <a href="../icon/superscript"><i class="fa fa-superscript"></i> superscript</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="subscript">
      <a href="../icon/subscript"><i class="fa fa-subscript"></i> subscript</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="eraser">
      <a href="../icon/eraser"><i class="fa fa-eraser"></i> eraser</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="puzzle-piece|addon|add-on|section">
      <a href="../icon/puzzle-piece"><i class="fa fa-puzzle-piece"></i> puzzle-piece</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="microphone|record|voice|sound">
      <a href="../icon/microphone"><i class="fa fa-microphone"></i> microphone</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="microphone-slash|record|voice|sound|mute">
      <a href="../icon/microphone-slash"><i class="fa fa-microphone-slash"></i> microphone-slash</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="shield|award|achievement|winner">
      <a href="../icon/shield"><i class="fa fa-shield"></i> shield</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="calendar-o|date|time|when">
      <a href="../icon/calendar-o"><i class="fa fa-calendar-o"></i> calendar-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="fire-extinguisher">
      <a href="../icon/fire-extinguisher"><i class="fa fa-fire-extinguisher"></i> fire-extinguisher</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="rocket|app">
      <a href="../icon/rocket"><i class="fa fa-rocket"></i> rocket</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="maxcdn">
      <a href="../icon/maxcdn"><i class="fa fa-maxcdn"></i> maxcdn</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="chevron-circle-left|previous|back">
      <a href="../icon/chevron-circle-left"><i class="fa fa-chevron-circle-left"></i> chevron-circle-left</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="chevron-circle-right|next|forward">
      <a href="../icon/chevron-circle-right"><i class="fa fa-chevron-circle-right"></i> chevron-circle-right</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="chevron-circle-up">
      <a href="../icon/chevron-circle-up"><i class="fa fa-chevron-circle-up"></i> chevron-circle-up</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="chevron-circle-down|more|dropdown|menu">
      <a href="../icon/chevron-circle-down"><i class="fa fa-chevron-circle-down"></i> chevron-circle-down</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="html5">
      <a href="../icon/html5"><i class="fa fa-html5"></i> html5</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="css3|code">
      <a href="../icon/css3"><i class="fa fa-css3"></i> css3</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="anchor|link">
      <a href="../icon/anchor"><i class="fa fa-anchor"></i> anchor</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="unlock-alt|protect|admin|password">
      <a href="../icon/unlock-alt"><i class="fa fa-unlock-alt"></i> unlock-alt</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="bullseye|target">
      <a href="../icon/bullseye"><i class="fa fa-bullseye"></i> bullseye</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="ellipsis-h|dots">
      <a href="../icon/ellipsis-h"><i class="fa fa-ellipsis-h"></i> ellipsis-h</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="ellipsis-v|dots">
      <a href="../icon/ellipsis-v"><i class="fa fa-ellipsis-v"></i> ellipsis-v</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="rss-square|feed|blog">
      <a href="../icon/rss-square"><i class="fa fa-rss-square"></i> rss-square</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="play-circle|start|playing">
      <a href="../icon/play-circle"><i class="fa fa-play-circle"></i> play-circle</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="ticket|movie|pass|support">
      <a href="../icon/ticket"><i class="fa fa-ticket"></i> ticket</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="minus-square|hide|minify|delete|remove|trash|hide|collapse">
      <a href="../icon/minus-square"><i class="fa fa-minus-square"></i> minus-square</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="minus-square-o|hide|minify|delete|remove|trash|hide|collapse">
      <a href="../icon/minus-square-o"><i class="fa fa-minus-square-o"></i> minus-square-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="level-up">
      <a href="../icon/level-up"><i class="fa fa-level-up"></i> level-up</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="level-down">
      <a href="../icon/level-down"><i class="fa fa-level-down"></i> level-down</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="check-square|checkmark|done|todo|agree|accept|confirm">
      <a href="../icon/check-square"><i class="fa fa-check-square"></i> check-square</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="pencil-square|write|edit|update">
      <a href="../icon/pencil-square"><i class="fa fa-pencil-square"></i> pencil-square</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="external-link-square|open|new">
      <a href="../icon/external-link-square"><i class="fa fa-external-link-square"></i> external-link-square</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="share-square|social|send">
      <a href="../icon/share-square"><i class="fa fa-share-square"></i> share-square</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="compass|safari|directory|menu|location">
      <a href="../icon/compass"><i class="fa fa-compass"></i> compass</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="caret-square-o-down|toggle-down|more|dropdown|menu">
      <a href="../icon/caret-square-o-down"><i class="fa fa-caret-square-o-down"></i> caret-square-o-down</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="caret-square-o-up|toggle-up">
      <a href="../icon/caret-square-o-up"><i class="fa fa-caret-square-o-up"></i> caret-square-o-up</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="caret-square-o-right|toggle-right|next|forward">
      <a href="../icon/caret-square-o-right"><i class="fa fa-caret-square-o-right"></i> caret-square-o-right</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="eur|euro">
      <a href="../icon/eur"><i class="fa fa-eur"></i> eur</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="gbp">
      <a href="../icon/gbp"><i class="fa fa-gbp"></i> gbp</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="usd|dollar">
      <a href="../icon/usd"><i class="fa fa-usd"></i> usd</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="inr|rupee">
      <a href="../icon/inr"><i class="fa fa-inr"></i> inr</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="jpy|cny|rmb|yen">
      <a href="../icon/jpy"><i class="fa fa-jpy"></i> jpy</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="rub|ruble|rouble">
      <a href="../icon/rub"><i class="fa fa-rub"></i> rub</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="krw|won">
      <a href="../icon/krw"><i class="fa fa-krw"></i> krw</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="btc|bitcoin">
      <a href="../icon/btc"><i class="fa fa-btc"></i> btc</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="file|new|page|pdf|document">
      <a href="../icon/file"><i class="fa fa-file"></i> file</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="file-text|new|page|pdf|document">
      <a href="../icon/file-text"><i class="fa fa-file-text"></i> file-text</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="sort-alpha-asc">
      <a href="../icon/sort-alpha-asc"><i class="fa fa-sort-alpha-asc"></i> sort-alpha-asc</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="sort-alpha-desc">
      <a href="../icon/sort-alpha-desc"><i class="fa fa-sort-alpha-desc"></i> sort-alpha-desc</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="sort-amount-asc">
      <a href="../icon/sort-amount-asc"><i class="fa fa-sort-amount-asc"></i> sort-amount-asc</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="sort-amount-desc">
      <a href="../icon/sort-amount-desc"><i class="fa fa-sort-amount-desc"></i> sort-amount-desc</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="sort-numeric-asc|numbers">
      <a href="../icon/sort-numeric-asc"><i class="fa fa-sort-numeric-asc"></i> sort-numeric-asc</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="sort-numeric-desc|numbers">
      <a href="../icon/sort-numeric-desc"><i class="fa fa-sort-numeric-desc"></i> sort-numeric-desc</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="thumbs-up|like|favorite|approve|agree|hand">
      <a href="../icon/thumbs-up"><i class="fa fa-thumbs-up"></i> thumbs-up</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="thumbs-down|dislike|disapprove|disagree|hand">
      <a href="../icon/thumbs-down"><i class="fa fa-thumbs-down"></i> thumbs-down</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="youtube-square|video|film">
      <a href="../icon/youtube-square"><i class="fa fa-youtube-square"></i> youtube-square</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="youtube|video|film">
      <a href="../icon/youtube"><i class="fa fa-youtube"></i> youtube</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="xing">
      <a href="../icon/xing"><i class="fa fa-xing"></i> xing</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="xing-square">
      <a href="../icon/xing-square"><i class="fa fa-xing-square"></i> xing-square</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="youtube-play|start|playing">
      <a href="../icon/youtube-play"><i class="fa fa-youtube-play"></i> youtube-play</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="dropbox">
      <a href="../icon/dropbox"><i class="fa fa-dropbox"></i> dropbox</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="stack-overflow">
      <a href="../icon/stack-overflow"><i class="fa fa-stack-overflow"></i> stack-overflow</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="instagram">
      <a href="../icon/instagram"><i class="fa fa-instagram"></i> instagram</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="flickr">
      <a href="../icon/flickr"><i class="fa fa-flickr"></i> flickr</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="adn">
      <a href="../icon/adn"><i class="fa fa-adn"></i> adn</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="bitbucket|git">
      <a href="../icon/bitbucket"><i class="fa fa-bitbucket"></i> bitbucket</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="bitbucket-square|git">
      <a href="../icon/bitbucket-square"><i class="fa fa-bitbucket-square"></i> bitbucket-square</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="tumblr">
      <a href="../icon/tumblr"><i class="fa fa-tumblr"></i> tumblr</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="tumblr-square">
      <a href="../icon/tumblr-square"><i class="fa fa-tumblr-square"></i> tumblr-square</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="long-arrow-down">
      <a href="../icon/long-arrow-down"><i class="fa fa-long-arrow-down"></i> long-arrow-down</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="long-arrow-up">
      <a href="../icon/long-arrow-up"><i class="fa fa-long-arrow-up"></i> long-arrow-up</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="long-arrow-left|previous|back">
      <a href="../icon/long-arrow-left"><i class="fa fa-long-arrow-left"></i> long-arrow-left</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="long-arrow-right">
      <a href="../icon/long-arrow-right"><i class="fa fa-long-arrow-right"></i> long-arrow-right</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="apple|osx">
      <a href="../icon/apple"><i class="fa fa-apple"></i> apple</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="windows|microsoft">
      <a href="../icon/windows"><i class="fa fa-windows"></i> windows</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="android">
      <a href="../icon/android"><i class="fa fa-android"></i> android</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="linux|tux">
      <a href="../icon/linux"><i class="fa fa-linux"></i> linux</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="dribbble">
      <a href="../icon/dribbble"><i class="fa fa-dribbble"></i> dribbble</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="skype">
      <a href="../icon/skype"><i class="fa fa-skype"></i> skype</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="foursquare">
      <a href="../icon/foursquare"><i class="fa fa-foursquare"></i> foursquare</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="trello">
      <a href="../icon/trello"><i class="fa fa-trello"></i> trello</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="female|woman|user|person|profile">
      <a href="../icon/female"><i class="fa fa-female"></i> female</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="male|man|user|person|profile">
      <a href="../icon/male"><i class="fa fa-male"></i> male</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="gratipay|gittip|heart|like|favorite|love">
      <a href="../icon/gratipay"><i class="fa fa-gratipay"></i> gratipay</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="sun-o|weather|contrast|lighter|brighten|day">
      <a href="../icon/sun-o"><i class="fa fa-sun-o"></i> sun-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="moon-o|night|darker|contrast">
      <a href="../icon/moon-o"><i class="fa fa-moon-o"></i> moon-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="archive|box|storage">
      <a href="../icon/archive"><i class="fa fa-archive"></i> archive</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="bug|report|insect">
      <a href="../icon/bug"><i class="fa fa-bug"></i> bug</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="vk">
      <a href="../icon/vk"><i class="fa fa-vk"></i> vk</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="weibo">
      <a href="../icon/weibo"><i class="fa fa-weibo"></i> weibo</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="renren">
      <a href="../icon/renren"><i class="fa fa-renren"></i> renren</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="pagelines|leaf|leaves|tree|plant|eco|nature">
      <a href="../icon/pagelines"><i class="fa fa-pagelines"></i> pagelines</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="stack-exchange">
      <a href="../icon/stack-exchange"><i class="fa fa-stack-exchange"></i> stack-exchange</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="arrow-circle-o-right|next|forward">
      <a href="../icon/arrow-circle-o-right"><i class="fa fa-arrow-circle-o-right"></i> arrow-circle-o-right</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="arrow-circle-o-left|previous|back">
      <a href="../icon/arrow-circle-o-left"><i class="fa fa-arrow-circle-o-left"></i> arrow-circle-o-left</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="caret-square-o-left|toggle-left|previous|back">
      <a href="../icon/caret-square-o-left"><i class="fa fa-caret-square-o-left"></i> caret-square-o-left</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="dot-circle-o|target|bullseye|notification">
      <a href="../icon/dot-circle-o"><i class="fa fa-dot-circle-o"></i> dot-circle-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="wheelchair|handicap|person|accessibility|accessibile">
      <a href="../icon/wheelchair"><i class="fa fa-wheelchair"></i> wheelchair</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="vimeo-square">
      <a href="../icon/vimeo-square"><i class="fa fa-vimeo-square"></i> vimeo-square</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="try|turkish-lira">
      <a href="../icon/try"><i class="fa fa-try"></i> try</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="plus-square-o|add|new|create|expand">
      <a href="../icon/plus-square-o"><i class="fa fa-plus-square-o"></i> plus-square-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="space-shuttle">
      <a href="../icon/space-shuttle"><i class="fa fa-space-shuttle"></i> space-shuttle</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="slack">
      <a href="../icon/slack"><i class="fa fa-slack"></i> slack</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="envelope-square">
      <a href="../icon/envelope-square"><i class="fa fa-envelope-square"></i> envelope-square</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="wordpress">
      <a href="../icon/wordpress"><i class="fa fa-wordpress"></i> wordpress</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="openid">
      <a href="../icon/openid"><i class="fa fa-openid"></i> openid</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="university|institution|bank">
      <a href="../icon/university"><i class="fa fa-university"></i> university</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="graduation-cap|mortar-board|learning|school|student">
      <a href="../icon/graduation-cap"><i class="fa fa-graduation-cap"></i> graduation-cap</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="yahoo">
      <a href="../icon/yahoo"><i class="fa fa-yahoo"></i> yahoo</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="google">
      <a href="../icon/google"><i class="fa fa-google"></i> google</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="reddit">
      <a href="../icon/reddit"><i class="fa fa-reddit"></i> reddit</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="reddit-square">
      <a href="../icon/reddit-square"><i class="fa fa-reddit-square"></i> reddit-square</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="stumbleupon-circle">
      <a href="../icon/stumbleupon-circle"><i class="fa fa-stumbleupon-circle"></i> stumbleupon-circle</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="stumbleupon">
      <a href="../icon/stumbleupon"><i class="fa fa-stumbleupon"></i> stumbleupon</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="delicious">
      <a href="../icon/delicious"><i class="fa fa-delicious"></i> delicious</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="digg">
      <a href="../icon/digg"><i class="fa fa-digg"></i> digg</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="pied-piper">
      <a href="../icon/pied-piper"><i class="fa fa-pied-piper"></i> pied-piper</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="pied-piper-alt">
      <a href="../icon/pied-piper-alt"><i class="fa fa-pied-piper-alt"></i> pied-piper-alt</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="drupal">
      <a href="../icon/drupal"><i class="fa fa-drupal"></i> drupal</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="joomla">
      <a href="../icon/joomla"><i class="fa fa-joomla"></i> joomla</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="language">
      <a href="../icon/language"><i class="fa fa-language"></i> language</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="fax">
      <a href="../icon/fax"><i class="fa fa-fax"></i> fax</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="building|work|business|apartment|office|company">
      <a href="../icon/building"><i class="fa fa-building"></i> building</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="child">
      <a href="../icon/child"><i class="fa fa-child"></i> child</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="paw|pet">
      <a href="../icon/paw"><i class="fa fa-paw"></i> paw</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="spoon">
      <a href="../icon/spoon"><i class="fa fa-spoon"></i> spoon</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="cube">
      <a href="../icon/cube"><i class="fa fa-cube"></i> cube</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="cubes">
      <a href="../icon/cubes"><i class="fa fa-cubes"></i> cubes</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="behance">
      <a href="../icon/behance"><i class="fa fa-behance"></i> behance</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="behance-square">
      <a href="../icon/behance-square"><i class="fa fa-behance-square"></i> behance-square</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="steam">
      <a href="../icon/steam"><i class="fa fa-steam"></i> steam</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="steam-square">
      <a href="../icon/steam-square"><i class="fa fa-steam-square"></i> steam-square</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="recycle">
      <a href="../icon/recycle"><i class="fa fa-recycle"></i> recycle</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="car|automobile|vehicle">
      <a href="../icon/car"><i class="fa fa-car"></i> car</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="taxi|cab|vehicle">
      <a href="../icon/taxi"><i class="fa fa-taxi"></i> taxi</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="tree">
      <a href="../icon/tree"><i class="fa fa-tree"></i> tree</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="spotify">
      <a href="../icon/spotify"><i class="fa fa-spotify"></i> spotify</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="deviantart">
      <a href="../icon/deviantart"><i class="fa fa-deviantart"></i> deviantart</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="soundcloud">
      <a href="../icon/soundcloud"><i class="fa fa-soundcloud"></i> soundcloud</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="database">
      <a href="../icon/database"><i class="fa fa-database"></i> database</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="file-pdf-o">
      <a href="../icon/file-pdf-o"><i class="fa fa-file-pdf-o"></i> file-pdf-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="file-word-o">
      <a href="../icon/file-word-o"><i class="fa fa-file-word-o"></i> file-word-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="file-excel-o">
      <a href="../icon/file-excel-o"><i class="fa fa-file-excel-o"></i> file-excel-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="file-powerpoint-o">
      <a href="../icon/file-powerpoint-o"><i class="fa fa-file-powerpoint-o"></i> file-powerpoint-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="file-image-o|file-photo-o|file-picture-o">
      <a href="../icon/file-image-o"><i class="fa fa-file-image-o"></i> file-image-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="file-archive-o|file-zip-o">
      <a href="../icon/file-archive-o"><i class="fa fa-file-archive-o"></i> file-archive-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="file-audio-o|file-sound-o">
      <a href="../icon/file-audio-o"><i class="fa fa-file-audio-o"></i> file-audio-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="file-video-o|file-movie-o">
      <a href="../icon/file-video-o"><i class="fa fa-file-video-o"></i> file-video-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="file-code-o">
      <a href="../icon/file-code-o"><i class="fa fa-file-code-o"></i> file-code-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="vine">
      <a href="../icon/vine"><i class="fa fa-vine"></i> vine</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="codepen">
      <a href="../icon/codepen"><i class="fa fa-codepen"></i> codepen</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="jsfiddle">
      <a href="../icon/jsfiddle"><i class="fa fa-jsfiddle"></i> jsfiddle</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="life-ring|life-bouy|life-buoy|life-saver|support">
      <a href="../icon/life-ring"><i class="fa fa-life-ring"></i> life-ring</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="circle-o-notch">
      <a href="../icon/circle-o-notch"><i class="fa fa-circle-o-notch"></i> circle-o-notch</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="rebel|ra">
      <a href="../icon/rebel"><i class="fa fa-rebel"></i> rebel</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="empire|ge">
      <a href="../icon/empire"><i class="fa fa-empire"></i> empire</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="git-square">
      <a href="../icon/git-square"><i class="fa fa-git-square"></i> git-square</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="git">
      <a href="../icon/git"><i class="fa fa-git"></i> git</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="hacker-news|y-combinator-square|yc-square">
      <a href="../icon/hacker-news"><i class="fa fa-hacker-news"></i> hacker-news</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="tencent-weibo">
      <a href="../icon/tencent-weibo"><i class="fa fa-tencent-weibo"></i> tencent-weibo</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="qq">
      <a href="../icon/qq"><i class="fa fa-qq"></i> qq</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="weixin|wechat">
      <a href="../icon/weixin"><i class="fa fa-weixin"></i> weixin</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="paper-plane|send">
      <a href="../icon/paper-plane"><i class="fa fa-paper-plane"></i> paper-plane</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="paper-plane-o|send-o">
      <a href="../icon/paper-plane-o"><i class="fa fa-paper-plane-o"></i> paper-plane-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="history">
      <a href="../icon/history"><i class="fa fa-history"></i> history</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="circle-thin">
      <a href="../icon/circle-thin"><i class="fa fa-circle-thin"></i> circle-thin</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="header">
      <a href="../icon/header"><i class="fa fa-header"></i> header</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="paragraph">
      <a href="../icon/paragraph"><i class="fa fa-paragraph"></i> paragraph</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="sliders">
      <a href="../icon/sliders"><i class="fa fa-sliders"></i> sliders</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="share-alt">
      <a href="../icon/share-alt"><i class="fa fa-share-alt"></i> share-alt</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="share-alt-square">
      <a href="../icon/share-alt-square"><i class="fa fa-share-alt-square"></i> share-alt-square</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="bomb">
      <a href="../icon/bomb"><i class="fa fa-bomb"></i> bomb</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="futbol-o|soccer-ball-o">
      <a href="../icon/futbol-o"><i class="fa fa-futbol-o"></i> futbol-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="tty">
      <a href="../icon/tty"><i class="fa fa-tty"></i> tty</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="binoculars">
      <a href="../icon/binoculars"><i class="fa fa-binoculars"></i> binoculars</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="plug">
      <a href="../icon/plug"><i class="fa fa-plug"></i> plug</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="slideshare">
      <a href="../icon/slideshare"><i class="fa fa-slideshare"></i> slideshare</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="twitch">
      <a href="../icon/twitch"><i class="fa fa-twitch"></i> twitch</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="yelp">
      <a href="../icon/yelp"><i class="fa fa-yelp"></i> yelp</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="newspaper-o">
      <a href="../icon/newspaper-o"><i class="fa fa-newspaper-o"></i> newspaper-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="wifi">
      <a href="../icon/wifi"><i class="fa fa-wifi"></i> wifi</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="calculator">
      <a href="../icon/calculator"><i class="fa fa-calculator"></i> calculator</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="paypal">
      <a href="../icon/paypal"><i class="fa fa-paypal"></i> paypal</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="google-wallet">
      <a href="../icon/google-wallet"><i class="fa fa-google-wallet"></i> google-wallet</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="cc-visa">
      <a href="../icon/cc-visa"><i class="fa fa-cc-visa"></i> cc-visa</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="cc-mastercard">
      <a href="../icon/cc-mastercard"><i class="fa fa-cc-mastercard"></i> cc-mastercard</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="cc-discover">
      <a href="../icon/cc-discover"><i class="fa fa-cc-discover"></i> cc-discover</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="cc-amex">
      <a href="../icon/cc-amex"><i class="fa fa-cc-amex"></i> cc-amex</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="cc-paypal">
      <a href="../icon/cc-paypal"><i class="fa fa-cc-paypal"></i> cc-paypal</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="cc-stripe">
      <a href="../icon/cc-stripe"><i class="fa fa-cc-stripe"></i> cc-stripe</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="bell-slash">
      <a href="../icon/bell-slash"><i class="fa fa-bell-slash"></i> bell-slash</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="bell-slash-o">
      <a href="../icon/bell-slash-o"><i class="fa fa-bell-slash-o"></i> bell-slash-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="trash">
      <a href="../icon/trash"><i class="fa fa-trash"></i> trash</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="copyright">
      <a href="../icon/copyright"><i class="fa fa-copyright"></i> copyright</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="at">
      <a href="../icon/at"><i class="fa fa-at"></i> at</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="eyedropper">
      <a href="../icon/eyedropper"><i class="fa fa-eyedropper"></i> eyedropper</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="paint-brush">
      <a href="../icon/paint-brush"><i class="fa fa-paint-brush"></i> paint-brush</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="birthday-cake">
      <a href="../icon/birthday-cake"><i class="fa fa-birthday-cake"></i> birthday-cake</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="area-chart">
      <a href="../icon/area-chart"><i class="fa fa-area-chart"></i> area-chart</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="pie-chart">
      <a href="../icon/pie-chart"><i class="fa fa-pie-chart"></i> pie-chart</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="line-chart">
      <a href="../icon/line-chart"><i class="fa fa-line-chart"></i> line-chart</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="lastfm">
      <a href="../icon/lastfm"><i class="fa fa-lastfm"></i> lastfm</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="lastfm-square">
      <a href="../icon/lastfm-square"><i class="fa fa-lastfm-square"></i> lastfm-square</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="toggle-off">
      <a href="../icon/toggle-off"><i class="fa fa-toggle-off"></i> toggle-off</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="toggle-on">
      <a href="../icon/toggle-on"><i class="fa fa-toggle-on"></i> toggle-on</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="bicycle|vehicle|bike">
      <a href="../icon/bicycle"><i class="fa fa-bicycle"></i> bicycle</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="bus|vehicle">
      <a href="../icon/bus"><i class="fa fa-bus"></i> bus</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="ioxhost">
      <a href="../icon/ioxhost"><i class="fa fa-ioxhost"></i> ioxhost</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="angellist">
      <a href="../icon/angellist"><i class="fa fa-angellist"></i> angellist</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="cc">
      <a href="../icon/cc"><i class="fa fa-cc"></i> cc</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="ils|shekel|sheqel">
      <a href="../icon/ils"><i class="fa fa-ils"></i> ils</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="meanpath">
      <a href="../icon/meanpath"><i class="fa fa-meanpath"></i> meanpath</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="buysellads">
      <a href="../icon/buysellads"><i class="fa fa-buysellads"></i> buysellads</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="connectdevelop">
      <a href="../icon/connectdevelop"><i class="fa fa-connectdevelop"></i> connectdevelop</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="dashcube">
      <a href="../icon/dashcube"><i class="fa fa-dashcube"></i> dashcube</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="forumbee">
      <a href="../icon/forumbee"><i class="fa fa-forumbee"></i> forumbee</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="leanpub">
      <a href="../icon/leanpub"><i class="fa fa-leanpub"></i> leanpub</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="sellsy">
      <a href="../icon/sellsy"><i class="fa fa-sellsy"></i> sellsy</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="shirtsinbulk">
      <a href="../icon/shirtsinbulk"><i class="fa fa-shirtsinbulk"></i> shirtsinbulk</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="simplybuilt">
      <a href="../icon/simplybuilt"><i class="fa fa-simplybuilt"></i> simplybuilt</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="skyatlas">
      <a href="../icon/skyatlas"><i class="fa fa-skyatlas"></i> skyatlas</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="cart-plus|add|shopping">
      <a href="../icon/cart-plus"><i class="fa fa-cart-plus"></i> cart-plus</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="cart-arrow-down|shopping">
      <a href="../icon/cart-arrow-down"><i class="fa fa-cart-arrow-down"></i> cart-arrow-down</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="diamond|gem|gemstone">
      <a href="../icon/diamond"><i class="fa fa-diamond"></i> diamond</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="ship|boat|sea">
      <a href="../icon/ship"><i class="fa fa-ship"></i> ship</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="user-secret|whisper|spy|incognito">
      <a href="../icon/user-secret"><i class="fa fa-user-secret"></i> user-secret</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="motorcycle|vehicle|bike">
      <a href="../icon/motorcycle"><i class="fa fa-motorcycle"></i> motorcycle</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="street-view|map">
      <a href="../icon/street-view"><i class="fa fa-street-view"></i> street-view</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="heartbeat|ekg">
      <a href="../icon/heartbeat"><i class="fa fa-heartbeat"></i> heartbeat</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="venus|female">
      <a href="../icon/venus"><i class="fa fa-venus"></i> venus</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="mars|male">
      <a href="../icon/mars"><i class="fa fa-mars"></i> mars</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="mercury|transgender">
      <a href="../icon/mercury"><i class="fa fa-mercury"></i> mercury</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="transgender|intersex">
      <a href="../icon/transgender"><i class="fa fa-transgender"></i> transgender</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="transgender-alt">
      <a href="../icon/transgender-alt"><i class="fa fa-transgender-alt"></i> transgender-alt</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="venus-double">
      <a href="../icon/venus-double"><i class="fa fa-venus-double"></i> venus-double</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="mars-double">
      <a href="../icon/mars-double"><i class="fa fa-mars-double"></i> mars-double</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="venus-mars">
      <a href="../icon/venus-mars"><i class="fa fa-venus-mars"></i> venus-mars</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="mars-stroke">
      <a href="../icon/mars-stroke"><i class="fa fa-mars-stroke"></i> mars-stroke</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="mars-stroke-v">
      <a href="../icon/mars-stroke-v"><i class="fa fa-mars-stroke-v"></i> mars-stroke-v</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="mars-stroke-h">
      <a href="../icon/mars-stroke-h"><i class="fa fa-mars-stroke-h"></i> mars-stroke-h</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="neuter">
      <a href="../icon/neuter"><i class="fa fa-neuter"></i> neuter</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="genderless">
      <a href="../icon/genderless"><i class="fa fa-genderless"></i> genderless</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="facebook-official">
      <a href="../icon/facebook-official"><i class="fa fa-facebook-official"></i> facebook-official</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="pinterest-p">
      <a href="../icon/pinterest-p"><i class="fa fa-pinterest-p"></i> pinterest-p</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="whatsapp">
      <a href="../icon/whatsapp"><i class="fa fa-whatsapp"></i> whatsapp</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="server">
      <a href="../icon/server"><i class="fa fa-server"></i> server</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="user-plus">
      <a href="../icon/user-plus"><i class="fa fa-user-plus"></i> user-plus</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="user-times">
      <a href="../icon/user-times"><i class="fa fa-user-times"></i> user-times</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="bed|hotel|travel">
      <a href="../icon/bed"><i class="fa fa-bed"></i> bed</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="viacoin">
      <a href="../icon/viacoin"><i class="fa fa-viacoin"></i> viacoin</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="train">
      <a href="../icon/train"><i class="fa fa-train"></i> train</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="subway">
      <a href="../icon/subway"><i class="fa fa-subway"></i> subway</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="medium">
      <a href="../icon/medium"><i class="fa fa-medium"></i> medium</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="y-combinator|yc">
      <a href="../icon/y-combinator"><i class="fa fa-y-combinator"></i> y-combinator</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="optin-monster">
      <a href="../icon/optin-monster"><i class="fa fa-optin-monster"></i> optin-monster</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="opencart">
      <a href="../icon/opencart"><i class="fa fa-opencart"></i> opencart</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="expeditedssl">
      <a href="../icon/expeditedssl"><i class="fa fa-expeditedssl"></i> expeditedssl</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="battery-full|battery-4">
      <a href="../icon/battery-full"><i class="fa fa-battery-full"></i> battery-full</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="battery-three-quarters|battery-3">
      <a href="../icon/battery-three-quarters"><i class="fa fa-battery-three-quarters"></i> battery-three-quarters</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="battery-half|battery-2">
      <a href="../icon/battery-half"><i class="fa fa-battery-half"></i> battery-half</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="battery-quarter|battery-1">
      <a href="../icon/battery-quarter"><i class="fa fa-battery-quarter"></i> battery-quarter</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="battery-empty|battery-0">
      <a href="../icon/battery-empty"><i class="fa fa-battery-empty"></i> battery-empty</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="mouse-pointer">
      <a href="../icon/mouse-pointer"><i class="fa fa-mouse-pointer"></i> mouse-pointer</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="i-cursor">
      <a href="../icon/i-cursor"><i class="fa fa-i-cursor"></i> i-cursor</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="object-group">
      <a href="../icon/object-group"><i class="fa fa-object-group"></i> object-group</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="object-ungroup">
      <a href="../icon/object-ungroup"><i class="fa fa-object-ungroup"></i> object-ungroup</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="sticky-note">
      <a href="../icon/sticky-note"><i class="fa fa-sticky-note"></i> sticky-note</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="sticky-note-o">
      <a href="../icon/sticky-note-o"><i class="fa fa-sticky-note-o"></i> sticky-note-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="cc-jcb">
      <a href="../icon/cc-jcb"><i class="fa fa-cc-jcb"></i> cc-jcb</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="cc-diners-club">
      <a href="../icon/cc-diners-club"><i class="fa fa-cc-diners-club"></i> cc-diners-club</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="clone|copy">
      <a href="../icon/clone"><i class="fa fa-clone"></i> clone</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="balance-scale">
      <a href="../icon/balance-scale"><i class="fa fa-balance-scale"></i> balance-scale</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="hourglass-o">
      <a href="../icon/hourglass-o"><i class="fa fa-hourglass-o"></i> hourglass-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="hourglass-start|hourglass-1">
      <a href="../icon/hourglass-start"><i class="fa fa-hourglass-start"></i> hourglass-start</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="hourglass-half|hourglass-2">
      <a href="../icon/hourglass-half"><i class="fa fa-hourglass-half"></i> hourglass-half</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="hourglass-end|hourglass-3">
      <a href="../icon/hourglass-end"><i class="fa fa-hourglass-end"></i> hourglass-end</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="hourglass">
      <a href="../icon/hourglass"><i class="fa fa-hourglass"></i> hourglass</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="hand-rock-o|hand-grab-o">
      <a href="../icon/hand-rock-o"><i class="fa fa-hand-rock-o"></i> hand-rock-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="hand-paper-o|hand-stop-o">
      <a href="../icon/hand-paper-o"><i class="fa fa-hand-paper-o"></i> hand-paper-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="hand-scissors-o">
      <a href="../icon/hand-scissors-o"><i class="fa fa-hand-scissors-o"></i> hand-scissors-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="hand-lizard-o">
      <a href="../icon/hand-lizard-o"><i class="fa fa-hand-lizard-o"></i> hand-lizard-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="hand-spock-o">
      <a href="../icon/hand-spock-o"><i class="fa fa-hand-spock-o"></i> hand-spock-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="hand-pointer-o">
      <a href="../icon/hand-pointer-o"><i class="fa fa-hand-pointer-o"></i> hand-pointer-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="hand-peace-o">
      <a href="../icon/hand-peace-o"><i class="fa fa-hand-peace-o"></i> hand-peace-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="trademark">
      <a href="../icon/trademark"><i class="fa fa-trademark"></i> trademark</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="registered">
      <a href="../icon/registered"><i class="fa fa-registered"></i> registered</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="creative-commons">
      <a href="../icon/creative-commons"><i class="fa fa-creative-commons"></i> creative-commons</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="gg">
      <a href="../icon/gg"><i class="fa fa-gg"></i> gg</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="gg-circle">
      <a href="../icon/gg-circle"><i class="fa fa-gg-circle"></i> gg-circle</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="tripadvisor">
      <a href="../icon/tripadvisor"><i class="fa fa-tripadvisor"></i> tripadvisor</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="odnoklassniki">
      <a href="../icon/odnoklassniki"><i class="fa fa-odnoklassniki"></i> odnoklassniki</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="odnoklassniki-square">
      <a href="../icon/odnoklassniki-square"><i class="fa fa-odnoklassniki-square"></i> odnoklassniki-square</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="get-pocket">
      <a href="../icon/get-pocket"><i class="fa fa-get-pocket"></i> get-pocket</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="wikipedia-w">
      <a href="../icon/wikipedia-w"><i class="fa fa-wikipedia-w"></i> wikipedia-w</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="safari">
      <a href="../icon/safari"><i class="fa fa-safari"></i> safari</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="chrome">
      <a href="../icon/chrome"><i class="fa fa-chrome"></i> chrome</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="firefox">
      <a href="../icon/firefox"><i class="fa fa-firefox"></i> firefox</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="opera">
      <a href="../icon/opera"><i class="fa fa-opera"></i> opera</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="internet-explorer">
      <a href="../icon/internet-explorer"><i class="fa fa-internet-explorer"></i> internet-explorer</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="television|tv">
      <a href="../icon/television"><i class="fa fa-television"></i> television</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="contao">
      <a href="../icon/contao"><i class="fa fa-contao"></i> contao</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="500px">
      <a href="../icon/500px"><i class="fa fa-500px"></i> 500px</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="amazon">
      <a href="../icon/amazon"><i class="fa fa-amazon"></i> amazon</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="calendar-plus-o">
      <a href="../icon/calendar-plus-o"><i class="fa fa-calendar-plus-o"></i> calendar-plus-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="calendar-minus-o">
      <a href="../icon/calendar-minus-o"><i class="fa fa-calendar-minus-o"></i> calendar-minus-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="calendar-times-o">
      <a href="../icon/calendar-times-o"><i class="fa fa-calendar-times-o"></i> calendar-times-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="calendar-check-o">
      <a href="../icon/calendar-check-o"><i class="fa fa-calendar-check-o"></i> calendar-check-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="industry">
      <a href="../icon/industry"><i class="fa fa-industry"></i> industry</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="map-pin">
      <a href="../icon/map-pin"><i class="fa fa-map-pin"></i> map-pin</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="map-signs">
      <a href="../icon/map-signs"><i class="fa fa-map-signs"></i> map-signs</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="map-o">
      <a href="../icon/map-o"><i class="fa fa-map-o"></i> map-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="map">
      <a href="../icon/map"><i class="fa fa-map"></i> map</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="commenting">
      <a href="../icon/commenting"><i class="fa fa-commenting"></i> commenting</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="commenting-o">
      <a href="../icon/commenting-o"><i class="fa fa-commenting-o"></i> commenting-o</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="houzz">
      <a href="../icon/houzz"><i class="fa fa-houzz"></i> houzz</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="vimeo">
      <a href="../icon/vimeo"><i class="fa fa-vimeo"></i> vimeo</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="black-tie">
      <a href="../icon/black-tie"><i class="fa fa-black-tie"></i> black-tie</a>
    </div>
    
    <div class="fa-hover col-md-3 col-sm-4 filter-icon" data-filter="fonticons">
      <a href="../icon/fonticons"><i class="fa fa-fonticons"></i> fonticons</a>
    </div>
    
  </div>
  <div id="no-search-results">
    <div class="alert alert-warning" role="alert"><i class="fa fa-warning margin-right-sm"></i>No icons matching <strong>'<span></span>'</strong> were found.</div>
  </div>
  <div class="alert alert-info" role="alert"><i class="fa fa-exclamation-circle margin-right-sm"></i>Tags are added by the community. Do you think your search query should return an icon? Send a pull request on <a href="https://github.com/FortAwesome/Font-Awesome/blob/master/CONTRIBUTING.md#suggesting-icon-keyword-additionremoval">GitHub</a>!</div>

</section>


  <section id="new">
  <h2 class="page-header">66 New Icons in 4.4</h2>
  

  <div class="row fontawesome-icon-list">
    

    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/500px"><i class="fa fa-500px"></i> 500px</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/amazon"><i class="fa fa-amazon"></i> amazon</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/balance-scale"><i class="fa fa-balance-scale"></i> balance-scale</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/battery-empty"><i class="fa fa-battery-0"></i> battery-0 <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/battery-quarter"><i class="fa fa-battery-1"></i> battery-1 <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/battery-half"><i class="fa fa-battery-2"></i> battery-2 <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/battery-three-quarters"><i class="fa fa-battery-3"></i> battery-3 <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/battery-full"><i class="fa fa-battery-4"></i> battery-4 <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/battery-empty"><i class="fa fa-battery-empty"></i> battery-empty</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/battery-full"><i class="fa fa-battery-full"></i> battery-full</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/battery-half"><i class="fa fa-battery-half"></i> battery-half</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/battery-quarter"><i class="fa fa-battery-quarter"></i> battery-quarter</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/battery-three-quarters"><i class="fa fa-battery-three-quarters"></i> battery-three-quarters</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/black-tie"><i class="fa fa-black-tie"></i> black-tie</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/calendar-check-o"><i class="fa fa-calendar-check-o"></i> calendar-check-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/calendar-minus-o"><i class="fa fa-calendar-minus-o"></i> calendar-minus-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/calendar-plus-o"><i class="fa fa-calendar-plus-o"></i> calendar-plus-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/calendar-times-o"><i class="fa fa-calendar-times-o"></i> calendar-times-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/cc-diners-club"><i class="fa fa-cc-diners-club"></i> cc-diners-club</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/cc-jcb"><i class="fa fa-cc-jcb"></i> cc-jcb</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/chrome"><i class="fa fa-chrome"></i> chrome</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/clone"><i class="fa fa-clone"></i> clone</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/commenting"><i class="fa fa-commenting"></i> commenting</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/commenting-o"><i class="fa fa-commenting-o"></i> commenting-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/contao"><i class="fa fa-contao"></i> contao</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/creative-commons"><i class="fa fa-creative-commons"></i> creative-commons</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/expeditedssl"><i class="fa fa-expeditedssl"></i> expeditedssl</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/firefox"><i class="fa fa-firefox"></i> firefox</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/fonticons"><i class="fa fa-fonticons"></i> fonticons</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/genderless"><i class="fa fa-genderless"></i> genderless</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/get-pocket"><i class="fa fa-get-pocket"></i> get-pocket</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/gg"><i class="fa fa-gg"></i> gg</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/gg-circle"><i class="fa fa-gg-circle"></i> gg-circle</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hand-rock-o"><i class="fa fa-hand-grab-o"></i> hand-grab-o <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hand-lizard-o"><i class="fa fa-hand-lizard-o"></i> hand-lizard-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hand-paper-o"><i class="fa fa-hand-paper-o"></i> hand-paper-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hand-peace-o"><i class="fa fa-hand-peace-o"></i> hand-peace-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hand-pointer-o"><i class="fa fa-hand-pointer-o"></i> hand-pointer-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hand-rock-o"><i class="fa fa-hand-rock-o"></i> hand-rock-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hand-scissors-o"><i class="fa fa-hand-scissors-o"></i> hand-scissors-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hand-spock-o"><i class="fa fa-hand-spock-o"></i> hand-spock-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hand-paper-o"><i class="fa fa-hand-stop-o"></i> hand-stop-o <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hourglass"><i class="fa fa-hourglass"></i> hourglass</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hourglass-start"><i class="fa fa-hourglass-1"></i> hourglass-1 <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hourglass-half"><i class="fa fa-hourglass-2"></i> hourglass-2 <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hourglass-end"><i class="fa fa-hourglass-3"></i> hourglass-3 <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hourglass-end"><i class="fa fa-hourglass-end"></i> hourglass-end</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hourglass-half"><i class="fa fa-hourglass-half"></i> hourglass-half</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hourglass-o"><i class="fa fa-hourglass-o"></i> hourglass-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hourglass-start"><i class="fa fa-hourglass-start"></i> hourglass-start</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/houzz"><i class="fa fa-houzz"></i> houzz</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/i-cursor"><i class="fa fa-i-cursor"></i> i-cursor</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/industry"><i class="fa fa-industry"></i> industry</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/internet-explorer"><i class="fa fa-internet-explorer"></i> internet-explorer</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/map"><i class="fa fa-map"></i> map</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/map-o"><i class="fa fa-map-o"></i> map-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/map-pin"><i class="fa fa-map-pin"></i> map-pin</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/map-signs"><i class="fa fa-map-signs"></i> map-signs</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/mouse-pointer"><i class="fa fa-mouse-pointer"></i> mouse-pointer</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/object-group"><i class="fa fa-object-group"></i> object-group</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/object-ungroup"><i class="fa fa-object-ungroup"></i> object-ungroup</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/odnoklassniki"><i class="fa fa-odnoklassniki"></i> odnoklassniki</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/odnoklassniki-square"><i class="fa fa-odnoklassniki-square"></i> odnoklassniki-square</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/opencart"><i class="fa fa-opencart"></i> opencart</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/opera"><i class="fa fa-opera"></i> opera</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/optin-monster"><i class="fa fa-optin-monster"></i> optin-monster</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/registered"><i class="fa fa-registered"></i> registered</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/safari"><i class="fa fa-safari"></i> safari</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/sticky-note"><i class="fa fa-sticky-note"></i> sticky-note</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/sticky-note-o"><i class="fa fa-sticky-note-o"></i> sticky-note-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/television"><i class="fa fa-television"></i> television</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/trademark"><i class="fa fa-trademark"></i> trademark</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/tripadvisor"><i class="fa fa-tripadvisor"></i> tripadvisor</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/television"><i class="fa fa-tv"></i> tv <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/vimeo"><i class="fa fa-vimeo"></i> vimeo</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/wikipedia-w"><i class="fa fa-wikipedia-w"></i> wikipedia-w</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/y-combinator"><i class="fa fa-y-combinator"></i> y-combinator</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/y-combinator"><i class="fa fa-yc"></i> yc <span class="text-muted">(alias)</span></a></div>
    
  </div>

</section>

  <section id="web-application">
  <h2 class="page-header">Web Application Icons</h2>

  <div class="row fontawesome-icon-list">
    

    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/adjust"><i class="fa fa-adjust"></i> adjust</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/anchor"><i class="fa fa-anchor"></i> anchor</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/archive"><i class="fa fa-archive"></i> archive</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/area-chart"><i class="fa fa-area-chart"></i> area-chart</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/arrows"><i class="fa fa-arrows"></i> arrows</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/arrows-h"><i class="fa fa-arrows-h"></i> arrows-h</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/arrows-v"><i class="fa fa-arrows-v"></i> arrows-v</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/asterisk"><i class="fa fa-asterisk"></i> asterisk</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/at"><i class="fa fa-at"></i> at</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/car"><i class="fa fa-automobile"></i> automobile <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/balance-scale"><i class="fa fa-balance-scale"></i> balance-scale</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/ban"><i class="fa fa-ban"></i> ban</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/university"><i class="fa fa-bank"></i> bank <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/bar-chart"><i class="fa fa-bar-chart"></i> bar-chart</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/bar-chart"><i class="fa fa-bar-chart-o"></i> bar-chart-o <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/barcode"><i class="fa fa-barcode"></i> barcode</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/bars"><i class="fa fa-bars"></i> bars</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/battery-empty"><i class="fa fa-battery-0"></i> battery-0 <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/battery-quarter"><i class="fa fa-battery-1"></i> battery-1 <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/battery-half"><i class="fa fa-battery-2"></i> battery-2 <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/battery-three-quarters"><i class="fa fa-battery-3"></i> battery-3 <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/battery-full"><i class="fa fa-battery-4"></i> battery-4 <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/battery-empty"><i class="fa fa-battery-empty"></i> battery-empty</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/battery-full"><i class="fa fa-battery-full"></i> battery-full</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/battery-half"><i class="fa fa-battery-half"></i> battery-half</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/battery-quarter"><i class="fa fa-battery-quarter"></i> battery-quarter</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/battery-three-quarters"><i class="fa fa-battery-three-quarters"></i> battery-three-quarters</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/bed"><i class="fa fa-bed"></i> bed</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/beer"><i class="fa fa-beer"></i> beer</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/bell"><i class="fa fa-bell"></i> bell</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/bell-o"><i class="fa fa-bell-o"></i> bell-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/bell-slash"><i class="fa fa-bell-slash"></i> bell-slash</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/bell-slash-o"><i class="fa fa-bell-slash-o"></i> bell-slash-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/bicycle"><i class="fa fa-bicycle"></i> bicycle</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/binoculars"><i class="fa fa-binoculars"></i> binoculars</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/birthday-cake"><i class="fa fa-birthday-cake"></i> birthday-cake</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/bolt"><i class="fa fa-bolt"></i> bolt</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/bomb"><i class="fa fa-bomb"></i> bomb</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/book"><i class="fa fa-book"></i> book</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/bookmark"><i class="fa fa-bookmark"></i> bookmark</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/bookmark-o"><i class="fa fa-bookmark-o"></i> bookmark-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/briefcase"><i class="fa fa-briefcase"></i> briefcase</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/bug"><i class="fa fa-bug"></i> bug</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/building"><i class="fa fa-building"></i> building</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/building-o"><i class="fa fa-building-o"></i> building-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/bullhorn"><i class="fa fa-bullhorn"></i> bullhorn</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/bullseye"><i class="fa fa-bullseye"></i> bullseye</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/bus"><i class="fa fa-bus"></i> bus</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/taxi"><i class="fa fa-cab"></i> cab <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/calculator"><i class="fa fa-calculator"></i> calculator</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/calendar"><i class="fa fa-calendar"></i> calendar</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/calendar-check-o"><i class="fa fa-calendar-check-o"></i> calendar-check-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/calendar-minus-o"><i class="fa fa-calendar-minus-o"></i> calendar-minus-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/calendar-o"><i class="fa fa-calendar-o"></i> calendar-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/calendar-plus-o"><i class="fa fa-calendar-plus-o"></i> calendar-plus-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/calendar-times-o"><i class="fa fa-calendar-times-o"></i> calendar-times-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/camera"><i class="fa fa-camera"></i> camera</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/camera-retro"><i class="fa fa-camera-retro"></i> camera-retro</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/car"><i class="fa fa-car"></i> car</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/caret-square-o-down"><i class="fa fa-caret-square-o-down"></i> caret-square-o-down</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/caret-square-o-left"><i class="fa fa-caret-square-o-left"></i> caret-square-o-left</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/caret-square-o-right"><i class="fa fa-caret-square-o-right"></i> caret-square-o-right</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/caret-square-o-up"><i class="fa fa-caret-square-o-up"></i> caret-square-o-up</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/cart-arrow-down"><i class="fa fa-cart-arrow-down"></i> cart-arrow-down</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/cart-plus"><i class="fa fa-cart-plus"></i> cart-plus</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/cc"><i class="fa fa-cc"></i> cc</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/certificate"><i class="fa fa-certificate"></i> certificate</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/check"><i class="fa fa-check"></i> check</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/check-circle"><i class="fa fa-check-circle"></i> check-circle</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/check-circle-o"><i class="fa fa-check-circle-o"></i> check-circle-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/check-square"><i class="fa fa-check-square"></i> check-square</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/check-square-o"><i class="fa fa-check-square-o"></i> check-square-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/child"><i class="fa fa-child"></i> child</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/circle"><i class="fa fa-circle"></i> circle</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/circle-o"><i class="fa fa-circle-o"></i> circle-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/circle-o-notch"><i class="fa fa-circle-o-notch"></i> circle-o-notch</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/circle-thin"><i class="fa fa-circle-thin"></i> circle-thin</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/clock-o"><i class="fa fa-clock-o"></i> clock-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/clone"><i class="fa fa-clone"></i> clone</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/times"><i class="fa fa-close"></i> close <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/cloud"><i class="fa fa-cloud"></i> cloud</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/cloud-download"><i class="fa fa-cloud-download"></i> cloud-download</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/cloud-upload"><i class="fa fa-cloud-upload"></i> cloud-upload</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/code"><i class="fa fa-code"></i> code</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/code-fork"><i class="fa fa-code-fork"></i> code-fork</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/coffee"><i class="fa fa-coffee"></i> coffee</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/cog"><i class="fa fa-cog"></i> cog</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/cogs"><i class="fa fa-cogs"></i> cogs</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/comment"><i class="fa fa-comment"></i> comment</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/comment-o"><i class="fa fa-comment-o"></i> comment-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/commenting"><i class="fa fa-commenting"></i> commenting</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/commenting-o"><i class="fa fa-commenting-o"></i> commenting-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/comments"><i class="fa fa-comments"></i> comments</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/comments-o"><i class="fa fa-comments-o"></i> comments-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/compass"><i class="fa fa-compass"></i> compass</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/copyright"><i class="fa fa-copyright"></i> copyright</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/creative-commons"><i class="fa fa-creative-commons"></i> creative-commons</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/credit-card"><i class="fa fa-credit-card"></i> credit-card</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/crop"><i class="fa fa-crop"></i> crop</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/crosshairs"><i class="fa fa-crosshairs"></i> crosshairs</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/cube"><i class="fa fa-cube"></i> cube</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/cubes"><i class="fa fa-cubes"></i> cubes</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/cutlery"><i class="fa fa-cutlery"></i> cutlery</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/tachometer"><i class="fa fa-dashboard"></i> dashboard <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/database"><i class="fa fa-database"></i> database</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/desktop"><i class="fa fa-desktop"></i> desktop</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/diamond"><i class="fa fa-diamond"></i> diamond</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/dot-circle-o"><i class="fa fa-dot-circle-o"></i> dot-circle-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/download"><i class="fa fa-download"></i> download</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/pencil-square-o"><i class="fa fa-edit"></i> edit <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/ellipsis-h"><i class="fa fa-ellipsis-h"></i> ellipsis-h</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/ellipsis-v"><i class="fa fa-ellipsis-v"></i> ellipsis-v</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/envelope"><i class="fa fa-envelope"></i> envelope</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/envelope-o"><i class="fa fa-envelope-o"></i> envelope-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/envelope-square"><i class="fa fa-envelope-square"></i> envelope-square</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/eraser"><i class="fa fa-eraser"></i> eraser</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/exchange"><i class="fa fa-exchange"></i> exchange</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/exclamation"><i class="fa fa-exclamation"></i> exclamation</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/exclamation-circle"><i class="fa fa-exclamation-circle"></i> exclamation-circle</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/exclamation-triangle"><i class="fa fa-exclamation-triangle"></i> exclamation-triangle</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/external-link"><i class="fa fa-external-link"></i> external-link</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/external-link-square"><i class="fa fa-external-link-square"></i> external-link-square</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/eye"><i class="fa fa-eye"></i> eye</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/eye-slash"><i class="fa fa-eye-slash"></i> eye-slash</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/eyedropper"><i class="fa fa-eyedropper"></i> eyedropper</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/fax"><i class="fa fa-fax"></i> fax</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/rss"><i class="fa fa-feed"></i> feed <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/female"><i class="fa fa-female"></i> female</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/fighter-jet"><i class="fa fa-fighter-jet"></i> fighter-jet</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/file-archive-o"><i class="fa fa-file-archive-o"></i> file-archive-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/file-audio-o"><i class="fa fa-file-audio-o"></i> file-audio-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/file-code-o"><i class="fa fa-file-code-o"></i> file-code-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/file-excel-o"><i class="fa fa-file-excel-o"></i> file-excel-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/file-image-o"><i class="fa fa-file-image-o"></i> file-image-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/file-video-o"><i class="fa fa-file-movie-o"></i> file-movie-o <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/file-pdf-o"><i class="fa fa-file-pdf-o"></i> file-pdf-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/file-image-o"><i class="fa fa-file-photo-o"></i> file-photo-o <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/file-image-o"><i class="fa fa-file-picture-o"></i> file-picture-o <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/file-powerpoint-o"><i class="fa fa-file-powerpoint-o"></i> file-powerpoint-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/file-audio-o"><i class="fa fa-file-sound-o"></i> file-sound-o <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/file-video-o"><i class="fa fa-file-video-o"></i> file-video-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/file-word-o"><i class="fa fa-file-word-o"></i> file-word-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/file-archive-o"><i class="fa fa-file-zip-o"></i> file-zip-o <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/film"><i class="fa fa-film"></i> film</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/filter"><i class="fa fa-filter"></i> filter</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/fire"><i class="fa fa-fire"></i> fire</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/fire-extinguisher"><i class="fa fa-fire-extinguisher"></i> fire-extinguisher</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/flag"><i class="fa fa-flag"></i> flag</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/flag-checkered"><i class="fa fa-flag-checkered"></i> flag-checkered</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/flag-o"><i class="fa fa-flag-o"></i> flag-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/bolt"><i class="fa fa-flash"></i> flash <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/flask"><i class="fa fa-flask"></i> flask</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/folder"><i class="fa fa-folder"></i> folder</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/folder-o"><i class="fa fa-folder-o"></i> folder-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/folder-open"><i class="fa fa-folder-open"></i> folder-open</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/folder-open-o"><i class="fa fa-folder-open-o"></i> folder-open-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/frown-o"><i class="fa fa-frown-o"></i> frown-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/futbol-o"><i class="fa fa-futbol-o"></i> futbol-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/gamepad"><i class="fa fa-gamepad"></i> gamepad</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/gavel"><i class="fa fa-gavel"></i> gavel</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/cog"><i class="fa fa-gear"></i> gear <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/cogs"><i class="fa fa-gears"></i> gears <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/gift"><i class="fa fa-gift"></i> gift</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/glass"><i class="fa fa-glass"></i> glass</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/globe"><i class="fa fa-globe"></i> globe</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/graduation-cap"><i class="fa fa-graduation-cap"></i> graduation-cap</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/users"><i class="fa fa-group"></i> group <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hand-rock-o"><i class="fa fa-hand-grab-o"></i> hand-grab-o <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hand-lizard-o"><i class="fa fa-hand-lizard-o"></i> hand-lizard-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hand-paper-o"><i class="fa fa-hand-paper-o"></i> hand-paper-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hand-peace-o"><i class="fa fa-hand-peace-o"></i> hand-peace-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hand-pointer-o"><i class="fa fa-hand-pointer-o"></i> hand-pointer-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hand-rock-o"><i class="fa fa-hand-rock-o"></i> hand-rock-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hand-scissors-o"><i class="fa fa-hand-scissors-o"></i> hand-scissors-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hand-spock-o"><i class="fa fa-hand-spock-o"></i> hand-spock-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hand-paper-o"><i class="fa fa-hand-stop-o"></i> hand-stop-o <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hdd-o"><i class="fa fa-hdd-o"></i> hdd-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/headphones"><i class="fa fa-headphones"></i> headphones</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/heart"><i class="fa fa-heart"></i> heart</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/heart-o"><i class="fa fa-heart-o"></i> heart-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/heartbeat"><i class="fa fa-heartbeat"></i> heartbeat</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/history"><i class="fa fa-history"></i> history</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/home"><i class="fa fa-home"></i> home</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/bed"><i class="fa fa-hotel"></i> hotel <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hourglass"><i class="fa fa-hourglass"></i> hourglass</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hourglass-start"><i class="fa fa-hourglass-1"></i> hourglass-1 <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hourglass-half"><i class="fa fa-hourglass-2"></i> hourglass-2 <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hourglass-end"><i class="fa fa-hourglass-3"></i> hourglass-3 <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hourglass-end"><i class="fa fa-hourglass-end"></i> hourglass-end</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hourglass-half"><i class="fa fa-hourglass-half"></i> hourglass-half</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hourglass-o"><i class="fa fa-hourglass-o"></i> hourglass-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hourglass-start"><i class="fa fa-hourglass-start"></i> hourglass-start</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/i-cursor"><i class="fa fa-i-cursor"></i> i-cursor</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/picture-o"><i class="fa fa-image"></i> image <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/inbox"><i class="fa fa-inbox"></i> inbox</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/industry"><i class="fa fa-industry"></i> industry</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/info"><i class="fa fa-info"></i> info</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/info-circle"><i class="fa fa-info-circle"></i> info-circle</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/university"><i class="fa fa-institution"></i> institution <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/key"><i class="fa fa-key"></i> key</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/keyboard-o"><i class="fa fa-keyboard-o"></i> keyboard-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/language"><i class="fa fa-language"></i> language</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/laptop"><i class="fa fa-laptop"></i> laptop</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/leaf"><i class="fa fa-leaf"></i> leaf</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/gavel"><i class="fa fa-legal"></i> legal <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/lemon-o"><i class="fa fa-lemon-o"></i> lemon-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/level-down"><i class="fa fa-level-down"></i> level-down</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/level-up"><i class="fa fa-level-up"></i> level-up</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/life-ring"><i class="fa fa-life-bouy"></i> life-bouy <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/life-ring"><i class="fa fa-life-buoy"></i> life-buoy <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/life-ring"><i class="fa fa-life-ring"></i> life-ring</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/life-ring"><i class="fa fa-life-saver"></i> life-saver <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/lightbulb-o"><i class="fa fa-lightbulb-o"></i> lightbulb-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/line-chart"><i class="fa fa-line-chart"></i> line-chart</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/location-arrow"><i class="fa fa-location-arrow"></i> location-arrow</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/lock"><i class="fa fa-lock"></i> lock</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/magic"><i class="fa fa-magic"></i> magic</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/magnet"><i class="fa fa-magnet"></i> magnet</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/share"><i class="fa fa-mail-forward"></i> mail-forward <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/reply"><i class="fa fa-mail-reply"></i> mail-reply <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/reply-all"><i class="fa fa-mail-reply-all"></i> mail-reply-all <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/male"><i class="fa fa-male"></i> male</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/map"><i class="fa fa-map"></i> map</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/map-marker"><i class="fa fa-map-marker"></i> map-marker</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/map-o"><i class="fa fa-map-o"></i> map-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/map-pin"><i class="fa fa-map-pin"></i> map-pin</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/map-signs"><i class="fa fa-map-signs"></i> map-signs</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/meh-o"><i class="fa fa-meh-o"></i> meh-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/microphone"><i class="fa fa-microphone"></i> microphone</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/microphone-slash"><i class="fa fa-microphone-slash"></i> microphone-slash</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/minus"><i class="fa fa-minus"></i> minus</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/minus-circle"><i class="fa fa-minus-circle"></i> minus-circle</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/minus-square"><i class="fa fa-minus-square"></i> minus-square</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/minus-square-o"><i class="fa fa-minus-square-o"></i> minus-square-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/mobile"><i class="fa fa-mobile"></i> mobile</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/mobile"><i class="fa fa-mobile-phone"></i> mobile-phone <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/money"><i class="fa fa-money"></i> money</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/moon-o"><i class="fa fa-moon-o"></i> moon-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/graduation-cap"><i class="fa fa-mortar-board"></i> mortar-board <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/motorcycle"><i class="fa fa-motorcycle"></i> motorcycle</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/mouse-pointer"><i class="fa fa-mouse-pointer"></i> mouse-pointer</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/music"><i class="fa fa-music"></i> music</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/bars"><i class="fa fa-navicon"></i> navicon <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/newspaper-o"><i class="fa fa-newspaper-o"></i> newspaper-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/object-group"><i class="fa fa-object-group"></i> object-group</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/object-ungroup"><i class="fa fa-object-ungroup"></i> object-ungroup</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/paint-brush"><i class="fa fa-paint-brush"></i> paint-brush</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/paper-plane"><i class="fa fa-paper-plane"></i> paper-plane</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/paper-plane-o"><i class="fa fa-paper-plane-o"></i> paper-plane-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/paw"><i class="fa fa-paw"></i> paw</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/pencil"><i class="fa fa-pencil"></i> pencil</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/pencil-square"><i class="fa fa-pencil-square"></i> pencil-square</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/pencil-square-o"><i class="fa fa-pencil-square-o"></i> pencil-square-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/phone"><i class="fa fa-phone"></i> phone</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/phone-square"><i class="fa fa-phone-square"></i> phone-square</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/picture-o"><i class="fa fa-photo"></i> photo <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/picture-o"><i class="fa fa-picture-o"></i> picture-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/pie-chart"><i class="fa fa-pie-chart"></i> pie-chart</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/plane"><i class="fa fa-plane"></i> plane</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/plug"><i class="fa fa-plug"></i> plug</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/plus"><i class="fa fa-plus"></i> plus</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/plus-circle"><i class="fa fa-plus-circle"></i> plus-circle</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/plus-square"><i class="fa fa-plus-square"></i> plus-square</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/plus-square-o"><i class="fa fa-plus-square-o"></i> plus-square-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/power-off"><i class="fa fa-power-off"></i> power-off</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/print"><i class="fa fa-print"></i> print</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/puzzle-piece"><i class="fa fa-puzzle-piece"></i> puzzle-piece</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/qrcode"><i class="fa fa-qrcode"></i> qrcode</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/question"><i class="fa fa-question"></i> question</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/question-circle"><i class="fa fa-question-circle"></i> question-circle</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/quote-left"><i class="fa fa-quote-left"></i> quote-left</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/quote-right"><i class="fa fa-quote-right"></i> quote-right</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/random"><i class="fa fa-random"></i> random</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/recycle"><i class="fa fa-recycle"></i> recycle</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/refresh"><i class="fa fa-refresh"></i> refresh</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/registered"><i class="fa fa-registered"></i> registered</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/times"><i class="fa fa-remove"></i> remove <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/bars"><i class="fa fa-reorder"></i> reorder <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/reply"><i class="fa fa-reply"></i> reply</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/reply-all"><i class="fa fa-reply-all"></i> reply-all</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/retweet"><i class="fa fa-retweet"></i> retweet</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/road"><i class="fa fa-road"></i> road</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/rocket"><i class="fa fa-rocket"></i> rocket</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/rss"><i class="fa fa-rss"></i> rss</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/rss-square"><i class="fa fa-rss-square"></i> rss-square</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/search"><i class="fa fa-search"></i> search</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/search-minus"><i class="fa fa-search-minus"></i> search-minus</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/search-plus"><i class="fa fa-search-plus"></i> search-plus</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/paper-plane"><i class="fa fa-send"></i> send <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/paper-plane-o"><i class="fa fa-send-o"></i> send-o <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/server"><i class="fa fa-server"></i> server</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/share"><i class="fa fa-share"></i> share</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/share-alt"><i class="fa fa-share-alt"></i> share-alt</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/share-alt-square"><i class="fa fa-share-alt-square"></i> share-alt-square</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/share-square"><i class="fa fa-share-square"></i> share-square</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/share-square-o"><i class="fa fa-share-square-o"></i> share-square-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/shield"><i class="fa fa-shield"></i> shield</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/ship"><i class="fa fa-ship"></i> ship</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/shopping-cart"><i class="fa fa-shopping-cart"></i> shopping-cart</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/sign-in"><i class="fa fa-sign-in"></i> sign-in</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/sign-out"><i class="fa fa-sign-out"></i> sign-out</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/signal"><i class="fa fa-signal"></i> signal</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/sitemap"><i class="fa fa-sitemap"></i> sitemap</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/sliders"><i class="fa fa-sliders"></i> sliders</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/smile-o"><i class="fa fa-smile-o"></i> smile-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/futbol-o"><i class="fa fa-soccer-ball-o"></i> soccer-ball-o <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/sort"><i class="fa fa-sort"></i> sort</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/sort-alpha-asc"><i class="fa fa-sort-alpha-asc"></i> sort-alpha-asc</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/sort-alpha-desc"><i class="fa fa-sort-alpha-desc"></i> sort-alpha-desc</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/sort-amount-asc"><i class="fa fa-sort-amount-asc"></i> sort-amount-asc</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/sort-amount-desc"><i class="fa fa-sort-amount-desc"></i> sort-amount-desc</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/sort-asc"><i class="fa fa-sort-asc"></i> sort-asc</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/sort-desc"><i class="fa fa-sort-desc"></i> sort-desc</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/sort-desc"><i class="fa fa-sort-down"></i> sort-down <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/sort-numeric-asc"><i class="fa fa-sort-numeric-asc"></i> sort-numeric-asc</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/sort-numeric-desc"><i class="fa fa-sort-numeric-desc"></i> sort-numeric-desc</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/sort-asc"><i class="fa fa-sort-up"></i> sort-up <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/space-shuttle"><i class="fa fa-space-shuttle"></i> space-shuttle</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/spinner"><i class="fa fa-spinner"></i> spinner</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/spoon"><i class="fa fa-spoon"></i> spoon</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/square"><i class="fa fa-square"></i> square</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/square-o"><i class="fa fa-square-o"></i> square-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/star"><i class="fa fa-star"></i> star</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/star-half"><i class="fa fa-star-half"></i> star-half</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/star-half-o"><i class="fa fa-star-half-empty"></i> star-half-empty <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/star-half-o"><i class="fa fa-star-half-full"></i> star-half-full <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/star-half-o"><i class="fa fa-star-half-o"></i> star-half-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/star-o"><i class="fa fa-star-o"></i> star-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/sticky-note"><i class="fa fa-sticky-note"></i> sticky-note</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/sticky-note-o"><i class="fa fa-sticky-note-o"></i> sticky-note-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/street-view"><i class="fa fa-street-view"></i> street-view</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/suitcase"><i class="fa fa-suitcase"></i> suitcase</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/sun-o"><i class="fa fa-sun-o"></i> sun-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/life-ring"><i class="fa fa-support"></i> support <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/tablet"><i class="fa fa-tablet"></i> tablet</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/tachometer"><i class="fa fa-tachometer"></i> tachometer</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/tag"><i class="fa fa-tag"></i> tag</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/tags"><i class="fa fa-tags"></i> tags</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/tasks"><i class="fa fa-tasks"></i> tasks</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/taxi"><i class="fa fa-taxi"></i> taxi</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/television"><i class="fa fa-television"></i> television</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/terminal"><i class="fa fa-terminal"></i> terminal</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/thumb-tack"><i class="fa fa-thumb-tack"></i> thumb-tack</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/thumbs-down"><i class="fa fa-thumbs-down"></i> thumbs-down</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/thumbs-o-down"><i class="fa fa-thumbs-o-down"></i> thumbs-o-down</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/thumbs-o-up"><i class="fa fa-thumbs-o-up"></i> thumbs-o-up</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/thumbs-up"><i class="fa fa-thumbs-up"></i> thumbs-up</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/ticket"><i class="fa fa-ticket"></i> ticket</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/times"><i class="fa fa-times"></i> times</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/times-circle"><i class="fa fa-times-circle"></i> times-circle</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/times-circle-o"><i class="fa fa-times-circle-o"></i> times-circle-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/tint"><i class="fa fa-tint"></i> tint</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/caret-square-o-down"><i class="fa fa-toggle-down"></i> toggle-down <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/caret-square-o-left"><i class="fa fa-toggle-left"></i> toggle-left <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/toggle-off"><i class="fa fa-toggle-off"></i> toggle-off</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/toggle-on"><i class="fa fa-toggle-on"></i> toggle-on</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/caret-square-o-right"><i class="fa fa-toggle-right"></i> toggle-right <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/caret-square-o-up"><i class="fa fa-toggle-up"></i> toggle-up <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/trademark"><i class="fa fa-trademark"></i> trademark</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/trash"><i class="fa fa-trash"></i> trash</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/trash-o"><i class="fa fa-trash-o"></i> trash-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/tree"><i class="fa fa-tree"></i> tree</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/trophy"><i class="fa fa-trophy"></i> trophy</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/truck"><i class="fa fa-truck"></i> truck</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/tty"><i class="fa fa-tty"></i> tty</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/television"><i class="fa fa-tv"></i> tv <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/umbrella"><i class="fa fa-umbrella"></i> umbrella</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/university"><i class="fa fa-university"></i> university</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/unlock"><i class="fa fa-unlock"></i> unlock</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/unlock-alt"><i class="fa fa-unlock-alt"></i> unlock-alt</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/sort"><i class="fa fa-unsorted"></i> unsorted <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/upload"><i class="fa fa-upload"></i> upload</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/user"><i class="fa fa-user"></i> user</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/user-plus"><i class="fa fa-user-plus"></i> user-plus</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/user-secret"><i class="fa fa-user-secret"></i> user-secret</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/user-times"><i class="fa fa-user-times"></i> user-times</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/users"><i class="fa fa-users"></i> users</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/video-camera"><i class="fa fa-video-camera"></i> video-camera</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/volume-down"><i class="fa fa-volume-down"></i> volume-down</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/volume-off"><i class="fa fa-volume-off"></i> volume-off</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/volume-up"><i class="fa fa-volume-up"></i> volume-up</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/exclamation-triangle"><i class="fa fa-warning"></i> warning <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/wheelchair"><i class="fa fa-wheelchair"></i> wheelchair</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/wifi"><i class="fa fa-wifi"></i> wifi</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/wrench"><i class="fa fa-wrench"></i> wrench</a></div>
    
  </div>

</section>

  <section id="hand">
  <h2 class="page-header">Hand Icons</h2>

  <div class="row fontawesome-icon-list">
    

    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hand-rock-o"><i class="fa fa-hand-grab-o"></i> hand-grab-o <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hand-lizard-o"><i class="fa fa-hand-lizard-o"></i> hand-lizard-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hand-o-down"><i class="fa fa-hand-o-down"></i> hand-o-down</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hand-o-left"><i class="fa fa-hand-o-left"></i> hand-o-left</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hand-o-right"><i class="fa fa-hand-o-right"></i> hand-o-right</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hand-o-up"><i class="fa fa-hand-o-up"></i> hand-o-up</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hand-paper-o"><i class="fa fa-hand-paper-o"></i> hand-paper-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hand-peace-o"><i class="fa fa-hand-peace-o"></i> hand-peace-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hand-pointer-o"><i class="fa fa-hand-pointer-o"></i> hand-pointer-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hand-rock-o"><i class="fa fa-hand-rock-o"></i> hand-rock-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hand-scissors-o"><i class="fa fa-hand-scissors-o"></i> hand-scissors-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hand-spock-o"><i class="fa fa-hand-spock-o"></i> hand-spock-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hand-paper-o"><i class="fa fa-hand-stop-o"></i> hand-stop-o <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/thumbs-down"><i class="fa fa-thumbs-down"></i> thumbs-down</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/thumbs-o-down"><i class="fa fa-thumbs-o-down"></i> thumbs-o-down</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/thumbs-o-up"><i class="fa fa-thumbs-o-up"></i> thumbs-o-up</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/thumbs-up"><i class="fa fa-thumbs-up"></i> thumbs-up</a></div>
    
  </div>

</section>

  <section id="transportation">
  <h2 class="page-header">Transportation Icons</h2>

  <div class="row fontawesome-icon-list">
    

    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/ambulance"><i class="fa fa-ambulance"></i> ambulance</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/car"><i class="fa fa-automobile"></i> automobile <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/bicycle"><i class="fa fa-bicycle"></i> bicycle</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/bus"><i class="fa fa-bus"></i> bus</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/taxi"><i class="fa fa-cab"></i> cab <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/car"><i class="fa fa-car"></i> car</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/fighter-jet"><i class="fa fa-fighter-jet"></i> fighter-jet</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/motorcycle"><i class="fa fa-motorcycle"></i> motorcycle</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/plane"><i class="fa fa-plane"></i> plane</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/rocket"><i class="fa fa-rocket"></i> rocket</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/ship"><i class="fa fa-ship"></i> ship</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/space-shuttle"><i class="fa fa-space-shuttle"></i> space-shuttle</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/subway"><i class="fa fa-subway"></i> subway</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/taxi"><i class="fa fa-taxi"></i> taxi</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/train"><i class="fa fa-train"></i> train</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/truck"><i class="fa fa-truck"></i> truck</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/wheelchair"><i class="fa fa-wheelchair"></i> wheelchair</a></div>
    
  </div>

</section>

  <section id="gender">
  <h2 class="page-header">Gender Icons</h2>

  <div class="row fontawesome-icon-list">
    

    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/genderless"><i class="fa fa-genderless"></i> genderless</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/transgender"><i class="fa fa-intersex"></i> intersex <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/mars"><i class="fa fa-mars"></i> mars</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/mars-double"><i class="fa fa-mars-double"></i> mars-double</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/mars-stroke"><i class="fa fa-mars-stroke"></i> mars-stroke</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/mars-stroke-h"><i class="fa fa-mars-stroke-h"></i> mars-stroke-h</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/mars-stroke-v"><i class="fa fa-mars-stroke-v"></i> mars-stroke-v</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/mercury"><i class="fa fa-mercury"></i> mercury</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/neuter"><i class="fa fa-neuter"></i> neuter</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/transgender"><i class="fa fa-transgender"></i> transgender</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/transgender-alt"><i class="fa fa-transgender-alt"></i> transgender-alt</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/venus"><i class="fa fa-venus"></i> venus</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/venus-double"><i class="fa fa-venus-double"></i> venus-double</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/venus-mars"><i class="fa fa-venus-mars"></i> venus-mars</a></div>
    
  </div>

</section>

  <section id="file-type">
  <h2 class="page-header">File Type Icons</h2>

  <div class="row fontawesome-icon-list">
    

    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/file"><i class="fa fa-file"></i> file</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/file-archive-o"><i class="fa fa-file-archive-o"></i> file-archive-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/file-audio-o"><i class="fa fa-file-audio-o"></i> file-audio-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/file-code-o"><i class="fa fa-file-code-o"></i> file-code-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/file-excel-o"><i class="fa fa-file-excel-o"></i> file-excel-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/file-image-o"><i class="fa fa-file-image-o"></i> file-image-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/file-video-o"><i class="fa fa-file-movie-o"></i> file-movie-o <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/file-o"><i class="fa fa-file-o"></i> file-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/file-pdf-o"><i class="fa fa-file-pdf-o"></i> file-pdf-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/file-image-o"><i class="fa fa-file-photo-o"></i> file-photo-o <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/file-image-o"><i class="fa fa-file-picture-o"></i> file-picture-o <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/file-powerpoint-o"><i class="fa fa-file-powerpoint-o"></i> file-powerpoint-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/file-audio-o"><i class="fa fa-file-sound-o"></i> file-sound-o <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/file-text"><i class="fa fa-file-text"></i> file-text</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/file-text-o"><i class="fa fa-file-text-o"></i> file-text-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/file-video-o"><i class="fa fa-file-video-o"></i> file-video-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/file-word-o"><i class="fa fa-file-word-o"></i> file-word-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/file-archive-o"><i class="fa fa-file-zip-o"></i> file-zip-o <span class="text-muted">(alias)</span></a></div>
    
  </div>

</section>

  <section id="spinner">
  <h2 class="page-header">Spinner Icons</h2>

  <div class="alert alert-success">
    <ul class="fa-ul">
      <li>
        <i class="fa fa-info-circle fa-lg fa-li"></i>
        These icons work great with the <code>fa-spin</code> class. Check out the
        <a href="../examples/#animated" class="alert-link">spinning icons example</a>.
      </li>
    </ul>
  </div>

  <div class="row fontawesome-icon-list">
    

    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/circle-o-notch"><i class="fa fa-circle-o-notch"></i> circle-o-notch</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/cog"><i class="fa fa-cog"></i> cog</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/cog"><i class="fa fa-gear"></i> gear <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/refresh"><i class="fa fa-refresh"></i> refresh</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/spinner"><i class="fa fa-spinner"></i> spinner</a></div>
    
  </div>
</section>

  <section id="form-control">
  <h2 class="page-header">Form Control Icons</h2>

  <div class="row fontawesome-icon-list">
    

    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/check-square"><i class="fa fa-check-square"></i> check-square</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/check-square-o"><i class="fa fa-check-square-o"></i> check-square-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/circle"><i class="fa fa-circle"></i> circle</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/circle-o"><i class="fa fa-circle-o"></i> circle-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/dot-circle-o"><i class="fa fa-dot-circle-o"></i> dot-circle-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/minus-square"><i class="fa fa-minus-square"></i> minus-square</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/minus-square-o"><i class="fa fa-minus-square-o"></i> minus-square-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/plus-square"><i class="fa fa-plus-square"></i> plus-square</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/plus-square-o"><i class="fa fa-plus-square-o"></i> plus-square-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/square"><i class="fa fa-square"></i> square</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/square-o"><i class="fa fa-square-o"></i> square-o</a></div>
    
  </div>
</section>

  <section id="payment">
  <h2 class="page-header">Payment Icons</h2>

  <div class="row fontawesome-icon-list">
    

    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/cc-amex"><i class="fa fa-cc-amex"></i> cc-amex</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/cc-diners-club"><i class="fa fa-cc-diners-club"></i> cc-diners-club</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/cc-discover"><i class="fa fa-cc-discover"></i> cc-discover</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/cc-jcb"><i class="fa fa-cc-jcb"></i> cc-jcb</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/cc-mastercard"><i class="fa fa-cc-mastercard"></i> cc-mastercard</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/cc-paypal"><i class="fa fa-cc-paypal"></i> cc-paypal</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/cc-stripe"><i class="fa fa-cc-stripe"></i> cc-stripe</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/cc-visa"><i class="fa fa-cc-visa"></i> cc-visa</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/credit-card"><i class="fa fa-credit-card"></i> credit-card</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/google-wallet"><i class="fa fa-google-wallet"></i> google-wallet</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/paypal"><i class="fa fa-paypal"></i> paypal</a></div>
    
  </div>

</section>

  <section id="chart">
  <h2 class="page-header">Chart Icons</h2>

  <div class="row fontawesome-icon-list">
    

    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/area-chart"><i class="fa fa-area-chart"></i> area-chart</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/bar-chart"><i class="fa fa-bar-chart"></i> bar-chart</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/bar-chart"><i class="fa fa-bar-chart-o"></i> bar-chart-o <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/line-chart"><i class="fa fa-line-chart"></i> line-chart</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/pie-chart"><i class="fa fa-pie-chart"></i> pie-chart</a></div>
    
  </div>

</section>

  <section id="currency">
  <h2 class="page-header">Currency Icons</h2>

  <div class="row fontawesome-icon-list">
    

    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/btc"><i class="fa fa-bitcoin"></i> bitcoin <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/btc"><i class="fa fa-btc"></i> btc</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/jpy"><i class="fa fa-cny"></i> cny <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/usd"><i class="fa fa-dollar"></i> dollar <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/eur"><i class="fa fa-eur"></i> eur</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/eur"><i class="fa fa-euro"></i> euro <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/gbp"><i class="fa fa-gbp"></i> gbp</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/gg"><i class="fa fa-gg"></i> gg</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/gg-circle"><i class="fa fa-gg-circle"></i> gg-circle</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/ils"><i class="fa fa-ils"></i> ils</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/inr"><i class="fa fa-inr"></i> inr</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/jpy"><i class="fa fa-jpy"></i> jpy</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/krw"><i class="fa fa-krw"></i> krw</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/money"><i class="fa fa-money"></i> money</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/jpy"><i class="fa fa-rmb"></i> rmb <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/rub"><i class="fa fa-rouble"></i> rouble <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/rub"><i class="fa fa-rub"></i> rub</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/rub"><i class="fa fa-ruble"></i> ruble <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/inr"><i class="fa fa-rupee"></i> rupee <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/ils"><i class="fa fa-shekel"></i> shekel <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/ils"><i class="fa fa-sheqel"></i> sheqel <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/try"><i class="fa fa-try"></i> try</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/try"><i class="fa fa-turkish-lira"></i> turkish-lira <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/usd"><i class="fa fa-usd"></i> usd</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/krw"><i class="fa fa-won"></i> won <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/jpy"><i class="fa fa-yen"></i> yen <span class="text-muted">(alias)</span></a></div>
    
  </div>

</section>

  <section id="text-editor">
  <h2 class="page-header">Text Editor Icons</h2>

  <div class="row fontawesome-icon-list">
    

    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/align-center"><i class="fa fa-align-center"></i> align-center</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/align-justify"><i class="fa fa-align-justify"></i> align-justify</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/align-left"><i class="fa fa-align-left"></i> align-left</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/align-right"><i class="fa fa-align-right"></i> align-right</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/bold"><i class="fa fa-bold"></i> bold</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/link"><i class="fa fa-chain"></i> chain <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/chain-broken"><i class="fa fa-chain-broken"></i> chain-broken</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/clipboard"><i class="fa fa-clipboard"></i> clipboard</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/columns"><i class="fa fa-columns"></i> columns</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/files-o"><i class="fa fa-copy"></i> copy <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/scissors"><i class="fa fa-cut"></i> cut <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/outdent"><i class="fa fa-dedent"></i> dedent <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/eraser"><i class="fa fa-eraser"></i> eraser</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/file"><i class="fa fa-file"></i> file</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/file-o"><i class="fa fa-file-o"></i> file-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/file-text"><i class="fa fa-file-text"></i> file-text</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/file-text-o"><i class="fa fa-file-text-o"></i> file-text-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/files-o"><i class="fa fa-files-o"></i> files-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/floppy-o"><i class="fa fa-floppy-o"></i> floppy-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/font"><i class="fa fa-font"></i> font</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/header"><i class="fa fa-header"></i> header</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/indent"><i class="fa fa-indent"></i> indent</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/italic"><i class="fa fa-italic"></i> italic</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/link"><i class="fa fa-link"></i> link</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/list"><i class="fa fa-list"></i> list</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/list-alt"><i class="fa fa-list-alt"></i> list-alt</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/list-ol"><i class="fa fa-list-ol"></i> list-ol</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/list-ul"><i class="fa fa-list-ul"></i> list-ul</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/outdent"><i class="fa fa-outdent"></i> outdent</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/paperclip"><i class="fa fa-paperclip"></i> paperclip</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/paragraph"><i class="fa fa-paragraph"></i> paragraph</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/clipboard"><i class="fa fa-paste"></i> paste <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/repeat"><i class="fa fa-repeat"></i> repeat</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/undo"><i class="fa fa-rotate-left"></i> rotate-left <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/repeat"><i class="fa fa-rotate-right"></i> rotate-right <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/floppy-o"><i class="fa fa-save"></i> save <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/scissors"><i class="fa fa-scissors"></i> scissors</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/strikethrough"><i class="fa fa-strikethrough"></i> strikethrough</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/subscript"><i class="fa fa-subscript"></i> subscript</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/superscript"><i class="fa fa-superscript"></i> superscript</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/table"><i class="fa fa-table"></i> table</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/text-height"><i class="fa fa-text-height"></i> text-height</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/text-width"><i class="fa fa-text-width"></i> text-width</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/th"><i class="fa fa-th"></i> th</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/th-large"><i class="fa fa-th-large"></i> th-large</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/th-list"><i class="fa fa-th-list"></i> th-list</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/underline"><i class="fa fa-underline"></i> underline</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/undo"><i class="fa fa-undo"></i> undo</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/chain-broken"><i class="fa fa-unlink"></i> unlink <span class="text-muted">(alias)</span></a></div>
    
  </div>

</section>

  <section id="directional">
  <h2 class="page-header">Directional Icons</h2>

  <div class="row fontawesome-icon-list">
    

    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/angle-double-down"><i class="fa fa-angle-double-down"></i> angle-double-down</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/angle-double-left"><i class="fa fa-angle-double-left"></i> angle-double-left</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/angle-double-right"><i class="fa fa-angle-double-right"></i> angle-double-right</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/angle-double-up"><i class="fa fa-angle-double-up"></i> angle-double-up</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/angle-down"><i class="fa fa-angle-down"></i> angle-down</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/angle-left"><i class="fa fa-angle-left"></i> angle-left</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/angle-right"><i class="fa fa-angle-right"></i> angle-right</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/angle-up"><i class="fa fa-angle-up"></i> angle-up</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/arrow-circle-down"><i class="fa fa-arrow-circle-down"></i> arrow-circle-down</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/arrow-circle-left"><i class="fa fa-arrow-circle-left"></i> arrow-circle-left</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/arrow-circle-o-down"><i class="fa fa-arrow-circle-o-down"></i> arrow-circle-o-down</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/arrow-circle-o-left"><i class="fa fa-arrow-circle-o-left"></i> arrow-circle-o-left</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/arrow-circle-o-right"><i class="fa fa-arrow-circle-o-right"></i> arrow-circle-o-right</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/arrow-circle-o-up"><i class="fa fa-arrow-circle-o-up"></i> arrow-circle-o-up</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/arrow-circle-right"><i class="fa fa-arrow-circle-right"></i> arrow-circle-right</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/arrow-circle-up"><i class="fa fa-arrow-circle-up"></i> arrow-circle-up</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/arrow-down"><i class="fa fa-arrow-down"></i> arrow-down</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/arrow-left"><i class="fa fa-arrow-left"></i> arrow-left</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/arrow-right"><i class="fa fa-arrow-right"></i> arrow-right</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/arrow-up"><i class="fa fa-arrow-up"></i> arrow-up</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/arrows"><i class="fa fa-arrows"></i> arrows</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/arrows-alt"><i class="fa fa-arrows-alt"></i> arrows-alt</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/arrows-h"><i class="fa fa-arrows-h"></i> arrows-h</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/arrows-v"><i class="fa fa-arrows-v"></i> arrows-v</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/caret-down"><i class="fa fa-caret-down"></i> caret-down</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/caret-left"><i class="fa fa-caret-left"></i> caret-left</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/caret-right"><i class="fa fa-caret-right"></i> caret-right</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/caret-square-o-down"><i class="fa fa-caret-square-o-down"></i> caret-square-o-down</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/caret-square-o-left"><i class="fa fa-caret-square-o-left"></i> caret-square-o-left</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/caret-square-o-right"><i class="fa fa-caret-square-o-right"></i> caret-square-o-right</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/caret-square-o-up"><i class="fa fa-caret-square-o-up"></i> caret-square-o-up</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/caret-up"><i class="fa fa-caret-up"></i> caret-up</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/chevron-circle-down"><i class="fa fa-chevron-circle-down"></i> chevron-circle-down</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/chevron-circle-left"><i class="fa fa-chevron-circle-left"></i> chevron-circle-left</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/chevron-circle-right"><i class="fa fa-chevron-circle-right"></i> chevron-circle-right</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/chevron-circle-up"><i class="fa fa-chevron-circle-up"></i> chevron-circle-up</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/chevron-down"><i class="fa fa-chevron-down"></i> chevron-down</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/chevron-left"><i class="fa fa-chevron-left"></i> chevron-left</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/chevron-right"><i class="fa fa-chevron-right"></i> chevron-right</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/chevron-up"><i class="fa fa-chevron-up"></i> chevron-up</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/exchange"><i class="fa fa-exchange"></i> exchange</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hand-o-down"><i class="fa fa-hand-o-down"></i> hand-o-down</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hand-o-left"><i class="fa fa-hand-o-left"></i> hand-o-left</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hand-o-right"><i class="fa fa-hand-o-right"></i> hand-o-right</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hand-o-up"><i class="fa fa-hand-o-up"></i> hand-o-up</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/long-arrow-down"><i class="fa fa-long-arrow-down"></i> long-arrow-down</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/long-arrow-left"><i class="fa fa-long-arrow-left"></i> long-arrow-left</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/long-arrow-right"><i class="fa fa-long-arrow-right"></i> long-arrow-right</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/long-arrow-up"><i class="fa fa-long-arrow-up"></i> long-arrow-up</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/caret-square-o-down"><i class="fa fa-toggle-down"></i> toggle-down <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/caret-square-o-left"><i class="fa fa-toggle-left"></i> toggle-left <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/caret-square-o-right"><i class="fa fa-toggle-right"></i> toggle-right <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/caret-square-o-up"><i class="fa fa-toggle-up"></i> toggle-up <span class="text-muted">(alias)</span></a></div>
    
  </div>

</section>

  <section id="video-player">
  <h2 class="page-header">Video Player Icons</h2>

  <div class="row fontawesome-icon-list">
    

    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/arrows-alt"><i class="fa fa-arrows-alt"></i> arrows-alt</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/backward"><i class="fa fa-backward"></i> backward</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/compress"><i class="fa fa-compress"></i> compress</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/eject"><i class="fa fa-eject"></i> eject</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/expand"><i class="fa fa-expand"></i> expand</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/fast-backward"><i class="fa fa-fast-backward"></i> fast-backward</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/fast-forward"><i class="fa fa-fast-forward"></i> fast-forward</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/forward"><i class="fa fa-forward"></i> forward</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/pause"><i class="fa fa-pause"></i> pause</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/play"><i class="fa fa-play"></i> play</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/play-circle"><i class="fa fa-play-circle"></i> play-circle</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/play-circle-o"><i class="fa fa-play-circle-o"></i> play-circle-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/random"><i class="fa fa-random"></i> random</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/step-backward"><i class="fa fa-step-backward"></i> step-backward</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/step-forward"><i class="fa fa-step-forward"></i> step-forward</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/stop"><i class="fa fa-stop"></i> stop</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/youtube-play"><i class="fa fa-youtube-play"></i> youtube-play</a></div>
    
  </div>

</section>

  <section id="brand">
  <h2 class="page-header">Brand Icons</h2>

  <div class="alert alert-warning">
    <h4><i class="fa fa-warning"></i> Warning!</h4>
Apparently, Adblock Plus can remove Font Awesome brand icons with their "Remove Social
Media Buttons" setting. We will not use hacks to force them to display. Please
<a href="https://adblockplus.org/en/bugs" class="alert-link">report an issue with Adblock Plus</a> if you believe this to be
an error. To work around this, you'll need to modify the social icon class names.

  </div>

  <div class="row fontawesome-icon-list margin-bottom-lg">
    

    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/500px"><i class="fa fa-500px"></i> 500px</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/adn"><i class="fa fa-adn"></i> adn</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/amazon"><i class="fa fa-amazon"></i> amazon</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/android"><i class="fa fa-android"></i> android</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/angellist"><i class="fa fa-angellist"></i> angellist</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/apple"><i class="fa fa-apple"></i> apple</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/behance"><i class="fa fa-behance"></i> behance</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/behance-square"><i class="fa fa-behance-square"></i> behance-square</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/bitbucket"><i class="fa fa-bitbucket"></i> bitbucket</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/bitbucket-square"><i class="fa fa-bitbucket-square"></i> bitbucket-square</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/btc"><i class="fa fa-bitcoin"></i> bitcoin <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/black-tie"><i class="fa fa-black-tie"></i> black-tie</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/btc"><i class="fa fa-btc"></i> btc</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/buysellads"><i class="fa fa-buysellads"></i> buysellads</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/cc-amex"><i class="fa fa-cc-amex"></i> cc-amex</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/cc-diners-club"><i class="fa fa-cc-diners-club"></i> cc-diners-club</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/cc-discover"><i class="fa fa-cc-discover"></i> cc-discover</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/cc-jcb"><i class="fa fa-cc-jcb"></i> cc-jcb</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/cc-mastercard"><i class="fa fa-cc-mastercard"></i> cc-mastercard</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/cc-paypal"><i class="fa fa-cc-paypal"></i> cc-paypal</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/cc-stripe"><i class="fa fa-cc-stripe"></i> cc-stripe</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/cc-visa"><i class="fa fa-cc-visa"></i> cc-visa</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/chrome"><i class="fa fa-chrome"></i> chrome</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/codepen"><i class="fa fa-codepen"></i> codepen</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/connectdevelop"><i class="fa fa-connectdevelop"></i> connectdevelop</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/contao"><i class="fa fa-contao"></i> contao</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/css3"><i class="fa fa-css3"></i> css3</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/dashcube"><i class="fa fa-dashcube"></i> dashcube</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/delicious"><i class="fa fa-delicious"></i> delicious</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/deviantart"><i class="fa fa-deviantart"></i> deviantart</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/digg"><i class="fa fa-digg"></i> digg</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/dribbble"><i class="fa fa-dribbble"></i> dribbble</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/dropbox"><i class="fa fa-dropbox"></i> dropbox</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/drupal"><i class="fa fa-drupal"></i> drupal</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/empire"><i class="fa fa-empire"></i> empire</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/expeditedssl"><i class="fa fa-expeditedssl"></i> expeditedssl</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/facebook"><i class="fa fa-facebook"></i> facebook</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/facebook"><i class="fa fa-facebook-f"></i> facebook-f <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/facebook-official"><i class="fa fa-facebook-official"></i> facebook-official</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/facebook-square"><i class="fa fa-facebook-square"></i> facebook-square</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/firefox"><i class="fa fa-firefox"></i> firefox</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/flickr"><i class="fa fa-flickr"></i> flickr</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/fonticons"><i class="fa fa-fonticons"></i> fonticons</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/forumbee"><i class="fa fa-forumbee"></i> forumbee</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/foursquare"><i class="fa fa-foursquare"></i> foursquare</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/empire"><i class="fa fa-ge"></i> ge <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/get-pocket"><i class="fa fa-get-pocket"></i> get-pocket</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/gg"><i class="fa fa-gg"></i> gg</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/gg-circle"><i class="fa fa-gg-circle"></i> gg-circle</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/git"><i class="fa fa-git"></i> git</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/git-square"><i class="fa fa-git-square"></i> git-square</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/github"><i class="fa fa-github"></i> github</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/github-alt"><i class="fa fa-github-alt"></i> github-alt</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/github-square"><i class="fa fa-github-square"></i> github-square</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/gratipay"><i class="fa fa-gittip"></i> gittip <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/google"><i class="fa fa-google"></i> google</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/google-plus"><i class="fa fa-google-plus"></i> google-plus</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/google-plus-square"><i class="fa fa-google-plus-square"></i> google-plus-square</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/google-wallet"><i class="fa fa-google-wallet"></i> google-wallet</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/gratipay"><i class="fa fa-gratipay"></i> gratipay</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hacker-news"><i class="fa fa-hacker-news"></i> hacker-news</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/houzz"><i class="fa fa-houzz"></i> houzz</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/html5"><i class="fa fa-html5"></i> html5</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/instagram"><i class="fa fa-instagram"></i> instagram</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/internet-explorer"><i class="fa fa-internet-explorer"></i> internet-explorer</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/ioxhost"><i class="fa fa-ioxhost"></i> ioxhost</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/joomla"><i class="fa fa-joomla"></i> joomla</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/jsfiddle"><i class="fa fa-jsfiddle"></i> jsfiddle</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/lastfm"><i class="fa fa-lastfm"></i> lastfm</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/lastfm-square"><i class="fa fa-lastfm-square"></i> lastfm-square</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/leanpub"><i class="fa fa-leanpub"></i> leanpub</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/linkedin"><i class="fa fa-linkedin"></i> linkedin</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/linkedin-square"><i class="fa fa-linkedin-square"></i> linkedin-square</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/linux"><i class="fa fa-linux"></i> linux</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/maxcdn"><i class="fa fa-maxcdn"></i> maxcdn</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/meanpath"><i class="fa fa-meanpath"></i> meanpath</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/medium"><i class="fa fa-medium"></i> medium</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/odnoklassniki"><i class="fa fa-odnoklassniki"></i> odnoklassniki</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/odnoklassniki-square"><i class="fa fa-odnoklassniki-square"></i> odnoklassniki-square</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/opencart"><i class="fa fa-opencart"></i> opencart</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/openid"><i class="fa fa-openid"></i> openid</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/opera"><i class="fa fa-opera"></i> opera</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/optin-monster"><i class="fa fa-optin-monster"></i> optin-monster</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/pagelines"><i class="fa fa-pagelines"></i> pagelines</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/paypal"><i class="fa fa-paypal"></i> paypal</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/pied-piper"><i class="fa fa-pied-piper"></i> pied-piper</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/pied-piper-alt"><i class="fa fa-pied-piper-alt"></i> pied-piper-alt</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/pinterest"><i class="fa fa-pinterest"></i> pinterest</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/pinterest-p"><i class="fa fa-pinterest-p"></i> pinterest-p</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/pinterest-square"><i class="fa fa-pinterest-square"></i> pinterest-square</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/qq"><i class="fa fa-qq"></i> qq</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/rebel"><i class="fa fa-ra"></i> ra <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/rebel"><i class="fa fa-rebel"></i> rebel</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/reddit"><i class="fa fa-reddit"></i> reddit</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/reddit-square"><i class="fa fa-reddit-square"></i> reddit-square</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/renren"><i class="fa fa-renren"></i> renren</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/safari"><i class="fa fa-safari"></i> safari</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/sellsy"><i class="fa fa-sellsy"></i> sellsy</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/share-alt"><i class="fa fa-share-alt"></i> share-alt</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/share-alt-square"><i class="fa fa-share-alt-square"></i> share-alt-square</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/shirtsinbulk"><i class="fa fa-shirtsinbulk"></i> shirtsinbulk</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/simplybuilt"><i class="fa fa-simplybuilt"></i> simplybuilt</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/skyatlas"><i class="fa fa-skyatlas"></i> skyatlas</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/skype"><i class="fa fa-skype"></i> skype</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/slack"><i class="fa fa-slack"></i> slack</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/slideshare"><i class="fa fa-slideshare"></i> slideshare</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/soundcloud"><i class="fa fa-soundcloud"></i> soundcloud</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/spotify"><i class="fa fa-spotify"></i> spotify</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/stack-exchange"><i class="fa fa-stack-exchange"></i> stack-exchange</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/stack-overflow"><i class="fa fa-stack-overflow"></i> stack-overflow</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/steam"><i class="fa fa-steam"></i> steam</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/steam-square"><i class="fa fa-steam-square"></i> steam-square</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/stumbleupon"><i class="fa fa-stumbleupon"></i> stumbleupon</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/stumbleupon-circle"><i class="fa fa-stumbleupon-circle"></i> stumbleupon-circle</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/tencent-weibo"><i class="fa fa-tencent-weibo"></i> tencent-weibo</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/trello"><i class="fa fa-trello"></i> trello</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/tripadvisor"><i class="fa fa-tripadvisor"></i> tripadvisor</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/tumblr"><i class="fa fa-tumblr"></i> tumblr</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/tumblr-square"><i class="fa fa-tumblr-square"></i> tumblr-square</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/twitch"><i class="fa fa-twitch"></i> twitch</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/twitter"><i class="fa fa-twitter"></i> twitter</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/twitter-square"><i class="fa fa-twitter-square"></i> twitter-square</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/viacoin"><i class="fa fa-viacoin"></i> viacoin</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/vimeo"><i class="fa fa-vimeo"></i> vimeo</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/vimeo-square"><i class="fa fa-vimeo-square"></i> vimeo-square</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/vine"><i class="fa fa-vine"></i> vine</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/vk"><i class="fa fa-vk"></i> vk</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/weixin"><i class="fa fa-wechat"></i> wechat <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/weibo"><i class="fa fa-weibo"></i> weibo</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/weixin"><i class="fa fa-weixin"></i> weixin</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/whatsapp"><i class="fa fa-whatsapp"></i> whatsapp</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/wikipedia-w"><i class="fa fa-wikipedia-w"></i> wikipedia-w</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/windows"><i class="fa fa-windows"></i> windows</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/wordpress"><i class="fa fa-wordpress"></i> wordpress</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/xing"><i class="fa fa-xing"></i> xing</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/xing-square"><i class="fa fa-xing-square"></i> xing-square</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/y-combinator"><i class="fa fa-y-combinator"></i> y-combinator</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hacker-news"><i class="fa fa-y-combinator-square"></i> y-combinator-square <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/yahoo"><i class="fa fa-yahoo"></i> yahoo</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/y-combinator"><i class="fa fa-yc"></i> yc <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hacker-news"><i class="fa fa-yc-square"></i> yc-square <span class="text-muted">(alias)</span></a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/yelp"><i class="fa fa-yelp"></i> yelp</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/youtube"><i class="fa fa-youtube"></i> youtube</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/youtube-play"><i class="fa fa-youtube-play"></i> youtube-play</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/youtube-square"><i class="fa fa-youtube-square"></i> youtube-square</a></div>
    
  </div>

  <div class="alert alert-success">
    <ul class="margin-bottom-none padding-left-lg">
  <li>All brand icons are trademarks of their respective owners.</li>
  <li>The use of these trademarks does not indicate endorsement of the trademark holder by Font Awesome, nor vice versa.</li>
</ul>

  </div>
</section>

  <section id="medical">
  <h2 class="page-header">Medical Icons</h2>

  <div class="row fontawesome-icon-list">
    

    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/ambulance"><i class="fa fa-ambulance"></i> ambulance</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/h-square"><i class="fa fa-h-square"></i> h-square</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/heart"><i class="fa fa-heart"></i> heart</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/heart-o"><i class="fa fa-heart-o"></i> heart-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/heartbeat"><i class="fa fa-heartbeat"></i> heartbeat</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/hospital-o"><i class="fa fa-hospital-o"></i> hospital-o</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/medkit"><i class="fa fa-medkit"></i> medkit</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/plus-square"><i class="fa fa-plus-square"></i> plus-square</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/stethoscope"><i class="fa fa-stethoscope"></i> stethoscope</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/user-md"><i class="fa fa-user-md"></i> user-md</a></div>
    
      <div class="fa-hover col-md-3 col-sm-4"><a href="../icon/wheelchair"><i class="fa fa-wheelchair"></i> wheelchair</a></div>
    
  </div>

</section>

</div>
</asp:Content>
