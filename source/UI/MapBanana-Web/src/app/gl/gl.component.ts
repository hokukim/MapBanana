import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'app-gl',
  templateUrl: './gl.component.html',
  styleUrls: ['./gl.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class GlComponent implements OnInit {

  constructor(
    private httpClient: HttpClient) {

    httpClient.get('https://localhost:5001/api/campaign/campaigns').subscribe(response => {
      console.log(response);
    });
  }

  ngOnInit(): void {
  }
}
