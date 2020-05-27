import {
  Component,
  OnInit,
  Input,
  Output,
  EventEmitter,
  ViewEncapsulation,
  ViewChild,
  ViewContainerRef,
} from '@angular/core';

@Component({
  selector: 'app-button-with-loading-state',
  templateUrl: './button-with-loading-state.component.html',
  styleUrls: ['./button-with-loading-state.component.scss'],
  // encapsulation: ViewEncapsulation.None,
})
export class ButtonWithLoadingStateComponent implements OnInit {
  @ViewChild('template', { static: true })
  template;
  @Input()
  loading = false;
  @Input()
  loadingText = 'Loading';
  @Output()
  click: EventEmitter<void> = new EventEmitter<void>();

  constructor(private viewContainerRef: ViewContainerRef) {}

  ngOnInit() {
    this.viewContainerRef.createEmbeddedView(this.template);
  }

  onClick() {
    this.click.next();
  }
}
