//var data = [
//    { y: '2014', a: 50 },
//    { y: '2015', a: 65 },
//    { y: '2016', a: 50 },
//    { y: '2017', a: 75 },
//    { y: '2018', a: 80 },
//    { y: '2019', a: 90 },
//    { y: '2020', a: 100 },
//    { y: '2021', a: 115 },
//    { y: '2022', a: 120 },
//    { y: '2023', a: 145 },
//    { y: '2024', a: 160 }
//],
//    config = {
//        data: data,
//        xkey: 'y',
//        ykeys: ['a', 'b'],
//        labels: ['Total Income', 'Total Outcome'],
//        fillOpacity: 0.6,
//        hideHover: 'auto',
//        behaveLikeLine: true,
//        resize: true,
//        pointFillColors: ['#ffffff'],
//        pointStrokeColors: ['black'],
//        lineColors: ['gray', 'red']
//    };
//config.element = 'area-chart';

//Morris.Line(config);
//config.element = 'bar-chart';
//Morris.Bar(config);
//config.element = 'stacked';
//config.stacked = true;
//Morris.Bar(config);
//Morris.Donut({
//    element: 'pie-chart',
//    data: [
//        { label: "Friends", value: 30 },
//        { label: "Allies", value: 15 },
//        { label: "Enemies", value: 45 },
//        { label: "Neutral", value: 10 }
//    ]
//});
new Morris.Line({
    // ID of the element in which to draw the chart.
    element: 'myfirstchart',
    // Chart data records -- each entry in this array corresponds to a point on
    // the chart.
    data: [
        { year: '2008', value: 20 },
        { year: '2009', value: 10 },
        { year: '2010', value: 5 },
        { year: '2011', value: 5 },
        { year: '2012', value: 20 }
    ],
    // The name of the data record attribute that contains x-values.
    xkey: 'year',
    // A list of names of data record attributes that contain y-values.
    ykeys: ['value'],
    // Labels for the ykeys -- will be displayed when you hover over the
    // chart.
    labels: ['Value']
});