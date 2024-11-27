import { Component, Input, SimpleChanges } from '@angular/core';

@Component({
  selector: 'app-pie-score',
  templateUrl: './pie-score.component.html',
  styleUrl: './pie-score.component.css'
})
export class PieScoreComponent {
  @Input() score: number = 0; 
  strokeDashoffset: number = 283;

  ngOnInit(): void {
    this.updateStrokeDashoffset();
  }

  private updateStrokeDashoffset(): void {
    const circumference = 283;
    this.strokeDashoffset = circumference - (this.score / 100) * circumference;
  }
}
