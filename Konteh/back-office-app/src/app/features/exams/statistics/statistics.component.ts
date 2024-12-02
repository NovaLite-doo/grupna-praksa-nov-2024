import { Component, OnInit } from '@angular/core';
import { ExamClient, GetExamStatisticsExamStatisticsResponse } from '../../../api/api-reference';
import { ScaleType } from '@swimlane/ngx-charts';

@Component({
  selector: 'app-statistics',
  templateUrl: './statistics.component.html',
  styleUrl: './statistics.component.css'
})
export class StatisticsComponent implements OnInit {
  public statistics: GetExamStatisticsExamStatisticsResponse | null = null;
  errorMessage: string = '';
  
  public chartData: any[] = [];
  public chartView: [number, number] = [700, 400];

  public colorScheme = {
    domain: ['#5AA454', '#a10d0d'],  
    name: 'coolColors',  
    selectable: true,   
    group: ScaleType.Ordinal   
  };

  constructor(private examClient: ExamClient) {}

  ngOnInit(): void {
    this.getStatistics();
  }

  getStatistics(): void {
    this.examClient.getStatistics()
      .subscribe(
        (response: GetExamStatisticsExamStatisticsResponse) => {
          this.statistics = response;
          this.chartData = [
            {
              "name": "Above 50%",
              "value": response.above50Percent ?? 0
            },
            {
              "name": "Below 50%",
              "value": response.below50Percent ?? 0
            }
          ];
        },
        error => {
          this.errorMessage = 'Error fetching statistics: ' + error.message;
        }
      );
  }
}
