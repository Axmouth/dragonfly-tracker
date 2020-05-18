import { Component, OnInit, Input, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'app-message-area',
  templateUrl: './message-area.component.html',
  styleUrls: ['./message-area.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MessageAreaComponent implements OnInit {
  @Input()
  errors: string[] = [];
  @Input()
  success: string[];
  @Input()
  info: string[];
  @Input()
  loading = false;

  constructor() {}

  ngOnInit() {}
}
