import {Directive, ElementRef, Input} from '@angular/core';
import {Network} from 'vis'

@Directive({
  selector: '[appGraphVis]'
})
export class GraphVisDirective {

  network;

  constructor(private el: ElementRef) {
  }

  @Input() set appGraphVis(graphData) {
    console.log('graph data ', graphData);
    //const options = {};
    const options = {
      edges: {
        smooth: {
          forceDirection: "none"
        }
        //font: {align: 'top'}
      },
      physics: {
        barnesHut: {
          gravitationalConstant: -10200,
          springLength: 85
        },
        minVelocity: 0.75
      },
      nodes: {
        shape: 'dot',
        /*scaling: {
          label: {
            min: 8,
            max: 20
          }
        }*/
      }
    };

    if (!this.network) {
      this.network = new Network(this.el.nativeElement, graphData, options);
    }

  }

}
