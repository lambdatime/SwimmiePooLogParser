$(function() {
    //$.ajax({
    //    //type: "POST",
    //    type: "GET"
    //    //async: false,
    //    url: "http://bb15.nbc.local:27057/api/Workload/BeginProcessingLoad",
    //    dataType: 'json',
    //    //crossdomain: true,
    //    data: {
    //        WorkLines:
    //        [{ LogFile: 'safsadf1', LogFilePath: 'C:\\Logs\\VWEB64\\', LineNumber: 1, WorkLine: '2013-05-04 00:00:12 10.7.30.124 GET /heartbeat - 80 - 10.7.31.3 a10hm/1.0 200 0 0 46' },
    //            { LogFile: 'asfdasdf2', LogFilePath: 'C:\\Logs\\VWEB64\\', LineNumber: 2, WorkLine: '2013-05-04 00:00:14 10.7.30.124 GET /heartbeat - 80 - 10.7.31.2 a10hm/1.0 200 0 0 31' }]
    //    },
    //    //jsonpCallback: "localJsonpCallback"
    //})
    //    .done(function(msg) {
    //        alert("Work started: " + msg);
    //    }).fail(function(e) {
    //        console.error(e);
    //    });
    //$.ajax({
    //    type: "POST",
    //     url: 'http://localhost:59610/api/Workload/BeginProcessingLoad',
    //    //url: "http://bb15.nbc.local:27057/api/Workload/BeginProcessingLoad",
    //    dataType: "jsonp",
    //   // jsonp: "callbackname",
    //    crossDomain: true,
    //    data: {
    //        WorkLoad: {
    //            WorkLines:
    //            [
    //                { LogFile: 'u_ex130504.log', LogFilePath: 'C:\\Logs\\VWEB64\\', LineNumber: 5, WorkLine: '2013-05-04 00:00:00 10.7.30.124 GET /Shop Schools%5B0%5D=University%20of%20North%20Texas&Schools%5B1%5D=Eastern%20Michigan%20University&Schools%5B2%5D=Greenville%20Technical%20College&Sort=Relevance 80 - 10.7.10.237 Mozilla/5.0+(compatible;+Googlebot/2.1;++http://www.google.com/bot.html) 200 0 0 1625' },
    //                { LogFile: 'u_ex130504.log', LogFilePath: 'C:\\Logs\\VWEB64\\', LineNumber: 6, WorkLine: '2013-05-04 00:00:00 10.7.30.124 GET /Content/images/sidebar-error.gif - 80 - 10.7.10.237 Mozilla/5.0+(iPad;+CPU+OS+6_1_3+like+Mac+OS+X)+AppleWebKit/536.26+(KHTML,+like+Gecko)+Version/6.0+Mobile/10B329+Safari/8536.25 200 0 0 15' },
    //                { LogFile: 'u_ex130504.log', LogFilePath: 'C:\\Logs\\VWEB64\\', LineNumber: 7, WorkLine: '2013-05-04 00:00:01 10.7.30.124 GET /Content/images/colorbox_border.png - 80 - 10.7.10.237 Mozilla/5.0+(iPad;+CPU+OS+6_1_2+like+Mac+OS+X)+AppleWebKit/536.26+(KHTML,+like+Gecko)+Version/6.0+Mobile/10B146+Safari/8536.25 200 0 0 15' },
    //                //{ LogFile: 'u_ex130504.log', LogFilePath: 'C:\\Logs\\VWEB64\\', LineNumber: 8, WorkLine: '2013-05-04 00:00:02 10.7.30.124 GET /heartbeat - 80 - 10.7.31.3 a10hm/1.0 200 0 0 46' },
    //                //{ LogFile: 'u_ex130504.log', LogFilePath: 'C:\\Logs\\VWEB64\\', LineNumber: 9, WorkLine: '2013-05-04 00:00:04 10.7.30.124 GET /emporia-state-university - 80 - 10.7.10.237 Mozilla/5.0+(compatible;+MJ12bot/v1.4.3;+http://www.majestic12.co.uk/bot.php?+) 200 0 0 1453' },
    //                //{ LogFile: 'u_ex130504.log', LogFilePath: 'C:\\Logs\\VWEB64\\', LineNumber: 10, WorkLine: '2013-05-04 00:00:04 10.7.30.124 GET /heartbeat - 80 - 10.7.31.2 a10hm/1.0 200 0 0 46' },
    //                //{ LogFile: 'u_ex130504.log', LogFilePath: 'C:\\Logs\\VWEB64\\', LineNumber: 11, WorkLine: '2013-05-04 00:00:05 10.7.30.124 GET /Content/Images/search-button.png - 80 - 10.7.10.237 Mozilla/5.0+(iPhone;+CPU+iPhone+OS+6_1_3+like+Mac+OS+X)+AppleWebKit/536.26+(KHTML,+like+Gecko)+Version/6.0+Mobile/10B329+Safari/8536.25 200 0 0 0' },
    //                //{ LogFile: 'u_ex130504.log', LogFilePath: 'C:\\Logs\\VWEB64\\', LineNumber: 12, WorkLine: '2013-05-04 00:00:05 10.7.30.124 GET /Content/images/question-mark.png - 80 - 10.7.10.237 Mozilla/5.0+(iPhone;+CPU+iPhone+OS+6_1_3+like+Mac+OS+X)+AppleWebKit/536.26+(KHTML,+like+Gecko)+Version/6.0+Mobile/10B329+Safari/8536.25 200 0 0 31' },
    //                //{ LogFile: 'u_ex130504.log', LogFilePath: 'C:\\Logs\\VWEB64\\', LineNumber: 13, WorkLine: '2013-05-04 00:00:05 10.7.30.124 GET /Shop Schools%5B0%5D=Dyersburg%20State%20Community%20College%20-%20Dyersburg%20Campus&Schools%5B1%5D=Maryville%20College&Schools%5B2%5D=University%20of%20Wisconsin-Waukesha&Sort=Relevance 80 - 10.7.10.237 Mozilla/5.0+(compatible;+Googlebot/2.1;++http://www.google.com/bot.html) 200 0 0 1625' },
    //                //{ LogFile: 'u_ex130504.log', LogFilePath: 'C:\\Logs\\VWEB64\\', LineNumber: 14, WorkLine: '2013-05-04 00:00:07 10.7.30.124 GET /Content/print.css - 80 - 10.7.10.237 Mozilla/5.0+(Macintosh;+Intel+Mac+OS+X+10.7;+rv:19.0)+Gecko/20100101+Firefox/19.0 200 0 0 15' },
    //                //{ LogFile: 'u_ex130504.log', LogFilePath: 'C:\\Logs\\VWEB64\\', LineNumber: 15, WorkLine: '2013-05-04 00:00:07 10.7.30.124 GET /heartbeat - 80 - 10.7.31.3 a10hm/1.0 200 0 0 46' },
    //                //{ LogFile: 'u_ex130504.log', LogFilePath: 'C:\\Logs\\VWEB64\\', LineNumber: 16, WorkLine: '2013-05-04 00:00:07 10.7.30.124 GET /texas-tech-university ww=1 80 - 10.7.10.237 Mozilla/5.0+(compatible;+MJ12bot/v1.4.3;+http://www.majestic12.co.uk/bot.php?+) 302 0 0 31' },
    //                //{ LogFile: 'u_ex130504.log', LogFilePath: 'C:\\Logs\\VWEB64\\', LineNumber: 17, WorkLine: '2013-05-04 00:00:08 10.7.30.124 GET /Content/images/pulldown-bg.png - 80 - 10.7.10.237 Mozilla/5.0+(Macintosh;+Intel+Mac+OS+X+10.7;+rv:19.0)+Gecko/20100101+Firefox/19.0 200 0 0 15' },
    //                //{ LogFile: 'u_ex130504.log', LogFilePath: 'C:\\Logs\\VWEB64\\', LineNumber: 18, WorkLine: '2013-05-04 00:00:08 10.7.30.124 GET /heartbeat - 80 - 10.7.31.2 a10hm/1.0 200 0 0 31' },
    //                //{ LogFile: 'u_ex130504.log', LogFilePath: 'C:\\Logs\\VWEB64\\', LineNumber: 19, WorkLine: '2013-05-04 00:00:10 10.7.30.124 GET /combres.axd/siteJs/-1331549999/ - 80 - 10.7.10.237 Mozilla/5.0+(iPhone;+CPU+iPhone+OS+6_1_3+like+Mac+OS+X)+AppleWebKit/536.26+(KHTML,+like+Gecko)+Version/6.0+Mobile/10B329+Safari/8536.25 200 0 0 0' },
    //                //{ LogFile: 'u_ex130504.log', LogFilePath: 'C:\\Logs\\VWEB64\\', LineNumber: 20, WorkLine: '2013-05-04 00:00:10 10.7.30.124 GET /Shop Schools%5B0%5D=University%20of%20North%20Texas&Schools%5B1%5D=Eastern%20Michigan%20University&Schools%5B2%5D=Jackson%20State%20Community%20College&Sort=Relevance 80 - 10.7.10.237 Mozilla/5.0+(compatible;+Googlebot/2.1;++http://www.google.com/bot.html) 200 0 0 1375' },
    //                //{ LogFile: 'u_ex130504.log', LogFilePath: 'C:\\Logs\\VWEB64\\', LineNumber: 21, WorkLine: '2013-05-04 00:00:10 10.7.30.124 GET /Content/images/btn-overlay.png - 80 - 10.7.10.237 Mozilla/5.0+(iPhone;+CPU+iPhone+OS+6_1_3+like+Mac+OS+X)+AppleWebKit/536.26+(KHTML,+like+Gecko)+Version/6.0+Mobile/10B329+Safari/8536.25 200 0 0 15' },
    //                //{ LogFile: 'u_ex130504.log', LogFilePath: 'C:\\Logs\\VWEB64\\', LineNumber: 22, WorkLine: '2013-05-04 00:00:12 10.7.30.124 GET /Content/images/background-stripes-left.png - 80 - 10.7.10.237 Mozilla/5.0+(iPhone;+CPU+iPhone+OS+6_1_3+like+Mac+OS+X)+AppleWebKit/536.26+(KHTML,+like+Gecko)+Version/6.0+Mobile/10B329+Safari/8536.25 200 0 0 0' }

    //                //{ LogFile: 'safsadf1', LogFilePath: 'C:\\Logs\\VWEB64\\', LineNumber: 1, WorkLine: '2013-05-04 00:00:12 10.7.30.124 GET /heartbeat - 80 - 10.7.31.3 a10hm/1.0 200 0 0 46' },
    //                //{ LogFile: 'asfdasdf2', LogFilePath: 'C:\\Logs\\VWEB64\\', LineNumber: 2, WorkLine: '2013-05-04 00:00:14 10.7.30.124 GET /heartbeat - 80 - 10.7.31.2 a10hm/1.0 200 0 0 31' }
    //            ]
    //        }
    //    },
    //    //jsonpCallback: "localJsonpCallback",

    //    success: function (result) {
    //        alert("Work started: " + result);
    //    },
    //    error: function (arguments) {
    //        console.log(arguments);
    //    }
    //});
});


//function localJsonpCallback(json) {
    
//}