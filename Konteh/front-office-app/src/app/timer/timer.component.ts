import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-timer',
  templateUrl: './timer.component.html',
  styleUrl: './timer.component.css'
})
export class TimerComponent implements OnInit, OnDestroy{
  @Input() totalTime: number = 0;  
  @Output() timeExpired: EventEmitter<void> = new EventEmitter<void>();
  
  remainingTime: number = 0;
  private destroy$: Subject<void> = new Subject<void>();
  private timerInterval: any;

  ngOnInit(): void {
    this.remainingTime = this.totalTime;
    this.startTimer();
  }

  startTimer(): void {
    this.timerInterval = setInterval(() => {
      if (this.remainingTime > 0) {
        this.remainingTime--;
      } else {
        this.timeExpired.emit();
        clearInterval(this.timerInterval); 
      }
    }, 1000);
  }

  ngOnDestroy(): void {
    clearInterval(this.timerInterval); 
    this.destroy$.next();
    this.destroy$.complete();
  }

  get formattedTime(): string {
    const minutes = Math.floor(this.remainingTime / 60);
    const seconds = this.remainingTime % 60;
    return `${minutes}:${seconds < 10 ? '0' + seconds : seconds}`;
  }
}
